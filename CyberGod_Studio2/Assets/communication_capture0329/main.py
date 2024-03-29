import socket
import numpy as np
import cv2
import cvzone
from cvzone.HandTrackingModule import HandDetector
from cvzone.PoseModule import PoseDetector
import os


host = '127.0.0.1'
port = 5005

capture = cv2.VideoCapture(0)
poseDetector = PoseDetector()
handDetector = HandDetector(maxHands=2, detectionCon=0.8)
posList = []


class Point:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z


class DirectPoint(Point):
    def __init__(self, lmlist, n, inverse):
        for lm in lmlist:
            if lm[0] == n:
                self.x = lm[1]
                self.y = inverse - lm[2]
                self.z = lm[3]


p16: Point = Point(0, 0, 0)
p18: Point = Point(0, 0, 0)
p20: Point = Point(0, 0, 0)
p22: Point = Point(0, 0, 0)


class Position():  # 部位，存储了两个Point（这是可以的嘛）
    def __init__(self, e1, e2):
        self.e1 = e1
        self.e2 = e2


# cross_mult(A,B,C,D)计算AB,CD两个向量的叉乘值并返回对应值
def cross_mult(A, B, C, D):
    ABx = B.x - A.x
    ABy = B.y - A.y
    CDx = D.x - C.x
    CDy = D.y - C.y
    return ABx * CDy - CDx * ABy


# quick_judge(A,B,C,D)快速排斥,T-无法判断,F-一定不香蕉
def quick_judge(A, B, C, D):
    if (max(A.x, B.x) < min(C.x, D.x) or
            max(C.x, D.x) < min(A.x, B.x) or
            max(A.y, B.y) < min(C.y, D.y) or
            max(C.y, D.y) < min(A.y, B.y)):
        return False
    else:
        return True


# banana(A,B,C,D)判断线段AB,CD是否香蕉并返回一个布尔值
def banana(A, B, C, D):
    if not quick_judge(A, B, C, D):
        return False
    CAxCD = cross_mult(C, A, C, D)
    CBxCD = cross_mult(C, B, C, D)
    BCxBA = cross_mult(B, C, B, A)
    BDxBA = cross_mult(B, D, B, A)
    if (CAxCD * CBxCD < 0) and (BCxBA * BDxBA < 0):
        return True
    else:
        return False


# hand_touch(A,B)手部三条线段是否触碰部位AB
def hand_touch(position):
    # 延长手部线段
    p18new = Point(1.5 * p18.x - 0.5 * p16.x, 1.5 * p18.y - 0.5 * p16.y, 1.5 * p18.z - 0.5 * p16.z)
    p20new = Point(1.5 * p20.x - 0.5 * p16.x, 1.5 * p20.y - 0.5 * p16.y, 1.5 * p20.z - 0.5 * p16.z)
    p22new = Point(1.5 * p22.x - 0.5 * p16.x, 1.5 * p22.y - 0.5 * p16.y, 1.5 * p22.z - 0.5 * p16.z)
    # 判断香蕉
    banana1 = banana(position.e1, position.e2, p16, p18new)
    banana2 = banana(position.e1, position.e2, p16, p20new)
    banana3 = banana(position.e1, position.e2, p16, p22new)
    return banana1 or banana2 or banana3


def connect_unity(host, port):
    global sock
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    # sock = socket.socket()
    sock.connect((host, port))
    print('CONNECTED')


def send_to_unity(arr):
    arr_list = arr.flatten().tolist()  # numpy数组转换为list类型
    data = '' + ','.join([str(elem) for elem in arr_list]) + ''  # 每个float用,分割
    sock.sendall(bytes(data, encoding="utf-8"))  # 发送数据
    print("SEND TO UNITY:", arr_list)


def rec_from_unity():
    data = sock.recv(1024)
    data = str(data, encoding='utf-8')
    data = data.split(',')
    new_data = []
    for d in data:
        new_data.append(float(d))
    print('RECEIVE FROM UNITY:', new_data)
    return new_data


# 逐帧处理

while True:
    success, img = capture.read()
    if success:

        img = poseDetector.findPose(img)
        # img = cv2.flip(img, flipCode=1)
        # 镜像语句
        lmList, bboxInfo = poseDetector.findPosition(img, draw=False)
        # lmHandList, bboxHandInfo = handDetector.findHands(img)
        if bboxInfo:
            p11 = DirectPoint(lmList, 11, img.shape[0])
            p13 = DirectPoint(lmList, 13, img.shape[0])
            p15 = DirectPoint(lmList, 15, img.shape[0])
            p23 = DirectPoint(lmList, 23, img.shape[0])
            p24 = DirectPoint(lmList, 24, img.shape[0])
            p25 = DirectPoint(lmList, 25, img.shape[0])
            p26 = DirectPoint(lmList, 26, img.shape[0])
            # 初始化了五个部位对应的lm
            # [0]左上臂11,13;[1]左下臂13,15;[2]左大腿23,25;[3]右大腿24,26;[4]心脏*
            positionList = [Position(p11, p13), Position(p13, p15),
                            Position(p23, p25), Position(p24, p26)]

            # 初始化手部四个lm
            p16 = DirectPoint(lmList, 16, img.shape[0])
            p18 = DirectPoint(lmList, 18, img.shape[0])
            p20 = DirectPoint(lmList, 20, img.shape[0])
            p22 = DirectPoint(lmList, 22, img.shape[0])

            position = 99
            if hand_touch(positionList[0]):
                position = 0
            elif hand_touch(positionList[1]):
                position = 1
            elif hand_touch(positionList[2]):
                position = 2
            elif hand_touch(positionList[3]):
                position = 3
            else:
                position = 99
            # 逐帧手部触碰判定

            data = np.array([position])
            connect_unity(host, port)
            send_to_unity(data)
            rec_from_unity()

    #cv2.imshow("image", img)
    #if cv2.waitKey(1) & 0xFF == ord('q'):
        #break
capture.release()
cv2.destroyAllWindows()













