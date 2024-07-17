import socket
from shapely.geometry import Polygon
import math
import numpy as np
import mediapipe as mp
import cv2
import cvzone
from cvzone.HandTrackingModule import HandDetector
from cvzone.PoseModule import PoseDetector
import os
import sys
from google.protobuf.json_format import MessageToDict
import time
import logging

# 设置日志
logging.basicConfig(filename="app.log", level=logging.DEBUG, format='%(asctime)s %(message)s')

np.set_printoptions(suppress=True)

receiveHost = '127.0.0.1'
receivePort = 5005
sendHost = '127.0.0.1'
sendPort = 5006

# 创建并绑定UDP套接字以接收数据
receiveSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
receiveSock.bind((receiveHost, receivePort))

# 创建UDP套接字以发送数据
sendSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = (sendHost, sendPort)

logging.info("Waiting to receive data from Unity...")
print("Waiting to receive data from Unity...")

def get_sorted_camera_indices():
    index = 0
    camera_infos = []
    while True:
        cap = cv2.VideoCapture(index)
        if not cap.isOpened():
            logging.info(f"Camera index {index} is not available.")
            print(f"Camera index {index} is not available.")
            break

        cap.set(cv2.CAP_PROP_FRAME_WIDTH, 1920)
        cap.set(cv2.CAP_PROP_FRAME_HEIGHT, 1080)
        time.sleep(5)

        width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
        height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))

        attempts = 0
        max_attempts = 50
        while (width <= 16 or height <= 16) and attempts < max_attempts:
            time.sleep(0.5)
            width = int(cap.get(cv2.CAP_PROP_FRAME_WIDTH))
            height = int(cap.get(cv2.CAP_PROP_FRAME_HEIGHT))
            attempts += 1
            logging.info(f"Waiting for camera {index} resolution... Attempt {attempts}")
            logging.info(f"Width: {width}, Height: {height}")
            print(f"Waiting for camera {index} resolution... Attempt {attempts}")
            print(f"Width: {width}, Height: {height}")

        if width > 16 and height > 16:
            resolution = width * height
            camera_infos.append((index, f"Camera {index}", resolution))
            logging.info(f"Camera {index}: {index}, Resolution: {width} x {height}")
            print(f"Camera {index}: {index}, Resolution: {width} x {height}")

        cap.release()
        index += 1

    camera_infos.sort(key=lambda x: x[2], reverse=True)
    return [(info[0], info[1]) for info in camera_infos]

def switch_camera(index):
    global current_capture
    print("Switching camera to index", index)
    if current_capture is not None:
        current_capture.release()
    current_capture = cv2.VideoCapture(index)
    if not current_capture.isOpened():
        logging.error(f"Failed to open camera index {index}")
        print(f"Failed to open camera index {index}")
        sys.exit(1)
    logging.info(f"Switched to camera index {index}")
    print(f"Switched to camera index {index}")
    print("current_capture:", current_capture)

current_capture = None
camera_index = None

sorted_cameras = get_sorted_camera_indices()
logging.info("Available cameras (sorted by resolution):")
for idx, name in sorted_cameras:
    logging.info(f"{idx}: {name}")

ready_message = "Python ready"
sendSock.sendto(ready_message.encode('utf-8'), serverAddressPort)
logging.info("Sent ready message to Unity.")
print("Sent ready message to Unity.")

while camera_index is None:
    logging.info("Waiting for camera index...")
    data, addr = receiveSock.recvfrom(1024)
    try:
        received_index = int(data.decode('utf-8').split()[-1])
    except ValueError:
        logging.warning("Received invalid data, could not convert to integer")
        print("Received invalid data, could not convert to integer")
        continue

    if 0 <= received_index < len(sorted_cameras):
        camera_index = sorted_cameras[received_index][0]
    else:
        logging.warning(f"Invalid camera index received: {received_index}")
        print(f"Invalid camera index received: {received_index}")
        continue

    print(f"Received initial camera index: {camera_index}")
    try:
        switch_camera(camera_index)
    except Exception as e:
        logging.error(f"Failed to switch to camera index {camera_index}: {e}")
        print(f"Failed to switch to camera index {camera_index}: {e}")
        camera_index = None
    print("initial camera_index:", camera_index)
print("initialization done1")



poseDetector = PoseDetector()
posList = []

class Point:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z

    def get_xy(self):
        return (self.x, self.y)

    def whether_in_image(self):
        if self.x < 0 or self.x > img.shape[1] or self.y < 0 or self.y > img.shape[0]:
            return 0
        return 1

print("initialization done2")



class DirectPoint(Point):
    def __init__(self, lmlist, n, inverse):
        self.x = 0
        self.y = 0
        self.z = 0
        for lm in lmlist:
            if lm[0] == n:
                self.x = lm[1]
                self.y = lm[2]
                self.z = lm[3]

p16 = Point(0, 0, 0)
p18 = Point(0, 0, 0)
p20 = Point(0, 0, 0)
p22 = Point(0, 0, 0)
print("initialization done3")

class Position:
    def __init__(self, e1, e2):
        self.e1 = e1
        self.e2 = e2

    def length_of_vector(self):
        return math.sqrt(math.pow((self.e1.x - self.e2.x), 2) + math.pow((self.e1.y - self.e2.y), 2))

    def k_of_vextor(self):
        return self.e1 / self.e2

    def calculate_arm_box_trans_vector(self):
        return ((-1 / 4) * self.e2, (1 / 4) * self.e1)

    def get_arm_box(self):
        new_p = Position(self.e2.x - self.e1.x, self.e2.y - self.e1.y)
        t = new_p.calculate_arm_box_trans_vector()
        return [(int(self.e1.x + t[0]), int(self.e1.y + t[1])),
                (int(self.e2.x + t[0]), int(self.e2.y + t[1])),
                (int(self.e2.x - t[0]), int(self.e2.y - t[1])),
                (int(self.e1.x - t[0]), int(self.e1.y - t[1]))]

print("initialization done4")


def get_hand_bbox2():
    p17new = Point(int(1.5 * p17.x - 0.5 * p15.x), int(1.5 * p17.y - 0.5 * p15.y), int(1.5 * p17.z - 0.5 * p15.z))
    p19new = Point(int(1.5 * p19.x - 0.5 * p15.x), int(1.5 * p19.y - 0.5 * p15.y), int(1.5 * p19.z - 0.5 * p15.z))
    draw_box([p17.get_xy(), p17new.get_xy(), p19new.get_xy(), p19.get_xy()], (0, 100, 0))
    cv2.circle(img, (p17new.x, p17new.y), 5, (0, 0, 100), -1)
    return [p17.get_xy(), p17new.get_xy(), p19new.get_xy(), p19.get_xy()]


print("initialization done5")
def three_equal_point(p1, p2):
    p3 = Point(0, 0, 0)
    p4 = Point(0, 0, 0)
    p3.x = int((2 * p1.x + p2.x) / 3)
    p3.y = int((2 * p1.y + p2.y) / 3)
    p4.x = int((2 * p2.x + p1.x) / 3)
    p4.y = int((2 * p2.y + p1.y) / 3)
    return p3, p4


print("initialization done6")
def half_point(p1, p2):
    p3 = Point(0, 0, 0)
    p3.x = int((p1.x + p2.x) / 2)
    p3.y = int((p1.y + p2.y) / 2)
    return p3

print("initialization done7")

def intersection_area(poly1, poly2):
    if poly1.intersection(poly2):
        return poly1.intersection(poly2).area
    return 0

print("initialization done8")


def draw_box(vertexlist, color):
    thickness = 5
    cv2.line(img, vertexlist[0], vertexlist[1], color, thickness)
    cv2.line(img, vertexlist[1], vertexlist[2], color, thickness)
    cv2.line(img, vertexlist[2], vertexlist[3], color, thickness)
    cv2.line(img, vertexlist[3], vertexlist[0], color, thickness)

print("initialization done9")


if __name__ == "__main__":
    print("into main")
    print("into main")
    print("into main")
    print("into main")
    print("into main")
    print("into main")
    print("into main last")
    try:
        print("into main Loop")
        # switch_camera(camera_index)
        print("Switched to camera index: ", camera_index)
        print("Start to read from camera.")

        while True:
            receiveSock.settimeout(0.01)
            try:
                data, addr = receiveSock.recvfrom(1024)
                new_camera_index = int(data.decode('utf-8').split()[-1])
                if 0 <= new_camera_index < len(sorted_cameras):
                    logging.info(f"Received new camera index: {new_camera_index}")
                    camera_index = sorted_cameras[new_camera_index][0]
                    switch_camera(camera_index)
            except (socket.timeout, ValueError):
                pass

            # print("Start to capture read.")
            # logging.info("Start to capture read.")
            #
            success, img = current_capture.read()
            #
            # logging.info("CaptureRead from camera.")
            # print("CaptureRead from camera.")
            if not success:
                logging.error("Failed to read from camera.")
                print("Failed to read from camera.")
                #break
            logging.info("Successfully read a frame from the camera.")

            bbox_on = 0
            position = 99
            knee_in = 0
            hand_in = 0
            hand_x = 0
            hand_y = 0

            if success:
                img = cv2.flip(img, flipCode=1)
                img = poseDetector.findPose(img)
                lmList, bboxInfo = poseDetector.findPosition(img, draw=False)
                imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)

                if bboxInfo:
                    p11 = DirectPoint(lmList, 11, img.shape[0])
                    p12 = DirectPoint(lmList, 12, img.shape[0])
                    p0 = DirectPoint(lmList, 0, img.shape[0])
                    p9 = DirectPoint(lmList, 9, img.shape[0])
                    p10 = DirectPoint(lmList, 10, img.shape[0])
                    if Position(p11, p12).length_of_vector() > 100 and min(p10.y, p9.y) > p0.y:
                        bbox_on = 1
                        p14 = DirectPoint(lmList, 14, img.shape[0])
                        p16 = DirectPoint(lmList, 16, img.shape[0])
                        p23 = DirectPoint(lmList, 23, img.shape[0])
                        p24 = DirectPoint(lmList, 24, img.shape[0])
                        p25 = DirectPoint(lmList, 25, img.shape[0])
                        p26 = DirectPoint(lmList, 26, img.shape[0])
                        positionList = [Position(p12, p14), Position(p14, p16), Position(p23, p25), Position(p24, p26)]

                        p15 = DirectPoint(lmList, 15, img.shape[0])
                        p17 = DirectPoint(lmList, 17, img.shape[0])
                        p19 = DirectPoint(lmList, 19, img.shape[0])
                        p21 = DirectPoint(lmList, 21, img.shape[0])
                        p34, p36 = three_equal_point(p12, p24)
                        p33, p35 = three_equal_point(p11, p23)
                        p37 = half_point(p11, p12)
                        p38 = half_point(p34, p33)
                        p39 = half_point(p36, p35)
                        p40 = half_point(p23, p24)
                        handbox = get_hand_bbox2()
                        handBboxPoly2 = Polygon(handbox)
                        testPoly20 = Polygon([p37.get_xy(), p11.get_xy(), p33.get_xy(), p38.get_xy()])
                        testPoly21 = Polygon([p37.get_xy(), p12.get_xy(), p34.get_xy(), p38.get_xy()])
                        testPoly22 = Polygon([p38.get_xy(), p33.get_xy(), p35.get_xy(), p39.get_xy()])
                        testPoly23 = Polygon([p38.get_xy(), p34.get_xy(), p36.get_xy(), p39.get_xy()])
                        testPoly24 = Polygon([p39.get_xy(), p35.get_xy(), p23.get_xy(), p40.get_xy()])
                        testPoly25 = Polygon([p39.get_xy(), p40.get_xy(), p24.get_xy(), p36.get_xy()])
                        bodyPoly = Polygon([p12.get_xy(), p11.get_xy(), p23.get_xy(), p24.get_xy()])
                        s0_vertex = positionList[0].get_arm_box()
                        s1_vertex = positionList[1].get_arm_box()
                        s2_vertex = positionList[2].get_arm_box()
                        s3_vertex = positionList[3].get_arm_box()
                        Poly0 = Polygon(s0_vertex)
                        Poly1 = Polygon(s1_vertex)
                        Poly2 = Polygon(s2_vertex)
                        Poly3 = Polygon(s3_vertex)
                        if (handBboxPoly2.is_valid and testPoly20.is_valid and testPoly21.is_valid and testPoly22.is_valid and testPoly23.is_valid and testPoly24.is_valid and testPoly25.is_valid and bodyPoly.is_valid):
                            s0 = intersection_area(handBboxPoly2, Poly0)
                            s1 = intersection_area(handBboxPoly2, Poly1)
                            s2 = intersection_area(handBboxPoly2, Poly2)
                            s3 = intersection_area(handBboxPoly2, Poly3)
                            s20 = 0
                            s21 = 0
                            s22 = 0
                            s23 = 0
                            s24 = 0
                            s25 = 0
                            s20 = intersection_area(handBboxPoly2, testPoly20)
                            s21 = intersection_area(handBboxPoly2, testPoly21)
                            s22 = intersection_area(handBboxPoly2, testPoly22)
                            s23 = intersection_area(handBboxPoly2, testPoly23)
                            s24 = intersection_area(handBboxPoly2, testPoly24)
                            s25 = intersection_area(handBboxPoly2, testPoly25)
                            dict_position = [
                                {'position': 0, 'area': s0, 'vertex': s0_vertex},
                                {'position': 1, 'area': s1, 'vertex': s1_vertex},
                                {'position': 2, 'area': s2, 'vertex': s2_vertex},
                                {'position': 3, 'area': s3, 'vertex': s3_vertex},
                                {'position': 20, 'area': s20, 'vertex': [p37.get_xy(), p11.get_xy(), p33.get_xy(), p38.get_xy()]},
                                {'position': 21, 'area': s21, 'vertex': [p37.get_xy(), p12.get_xy(), p34.get_xy(), p38.get_xy()]},
                                {'position': 22, 'area': s22, 'vertex': [p38.get_xy(), p33.get_xy(), p35.get_xy(), p39.get_xy()]},
                                {'position': 23, 'area': s23, 'vertex': [p38.get_xy(), p34.get_xy(), p36.get_xy(), p39.get_xy()]},
                                {'position': 24, 'area': s24, 'vertex': [p39.get_xy(), p35.get_xy(), p23.get_xy(), p40.get_xy()]},
                                {'position': 25, 'area': s25, 'vertex': [p39.get_xy(), p40.get_xy(), p24.get_xy(), p36.get_xy()]}]
                            max_position = max(dict_position, key=lambda x: x['area'])
                            if max_position['area'] != 0:
                                position = max_position['position']
                                draw_box(max_position['vertex'], (255, 0, 0))

                            hand_in = p17.whether_in_image()
                            if hand_in == 1:
                                hand_x = handbox[0][0] / img.shape[1]
                                hand_x = round(hand_x, 4)
                                hand_y = (img.shape[0] - handbox[0][1]) / img.shape[0]
                                hand_y = round(hand_y, 4)
                            else:
                                hand_x = -1.
                                hand_y = -1.

                            knee_in = p26.whether_in_image() * p25.whether_in_image()

            data = np.array([bbox_on, position, knee_in, hand_in, hand_x, hand_y])
            logging.info(f"Sending data: {data}")
            sendSock.sendto(str.encode(str(data)), serverAddressPort)

            if cv2.waitKey(1) & 0xFF == ord('q'):
                print("Exit by user.")
                break

    except Exception as e:
        logging.exception("Exception occurred during main loop execution")
        print(f"Exception occurred: {e}")

    finally:
        if current_capture:
            current_capture.release()
        cv2.destroyAllWindows()
        logging.info("Application terminated.")
        print("Application terminated.")

# The following command can be used to create an executable file with PyInstaller:
# pyinstaller --add-data "/Volumes/Rooster_SSD/Anaconda/anaconda3/envs/cybergod/lib/python3.9/site-packages/mediapipe:mediapipe" --hidden-import cvzone --hidden-import mediapipe --hidden-import shapely._geos /Volumes/Rooster_SSD/_Unity_Projects/CyberGod_Studio2/CyberGod_Studio2_RW/CyberGod_Studio2/Assets/Scripts/bodydivide_test_v20402/main.py
# pyinstaller --noconfirm --add-data "F:\anaconda3_f\envs\brandnewCyberGod\Lib\site-packages\mediapipe:mediapipe" --hidden-import cvzone --hidden-import mediapipe --hidden-import shapely._geos D:\Unity\Cybergod_win_local\CyberGod_Studio2_RW\CyberGod_Studio2\Assets\Scripts\bodydivide_test_v20402\main.py