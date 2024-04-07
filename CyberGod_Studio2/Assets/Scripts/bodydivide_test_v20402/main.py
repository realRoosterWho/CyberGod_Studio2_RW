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
from google.protobuf.json_format import MessageToDict


host = '127.0.0.1'
port = 5005

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = (host, port)

#capture = cv2.VideoCapture(0)
#如果有多个ViedoCapturecapture，全部获取，然后看其中哪些图像大小不是0，就是有图像的，然后使得capture=这个
# 假设有多个视频捕获设备，我们用0, 1, 2...来代表它们
device_indices = [0, 1, 2]  # 可以根据实际情况增减
valid_captures = []

# 尝试从所有设备中捕获视频
for index in device_indices:
    cap = cv2.VideoCapture(index)
    if cap.isOpened():  # 检查是否成功打开摄像头
        ret, frame = cap.read()
        if ret and frame.size > 0:  # 检查图像大小是否不为0
            valid_captures.append(cap)
        else:
            cap.release()  # 释放无效的视频捕获对象

# 选择第一个有效的视频捕获对象
if valid_captures:
    capture = valid_captures[0]
    print(f"Using device with index {device_indices[valid_captures.index(capture)]}")
else:
    print("No valid video capture device found.")

poseDetector = PoseDetector()
mpHands = mp.solutions.hands  # 接收方法
hands = mpHands.Hands(static_image_mode=False,  # 静态追踪，低于0.5置信度会再一次跟踪
                      max_num_hands= 2,  # 最多有2只手
                      min_detection_confidence=0.05,  # 最小检测置信度
                      min_tracking_confidence=0.2)  # 最小跟踪置信度

mpDraw = mp.solutions.drawing_utils

posList = []


class Point:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z
    def get_xy(self):
        return (self.x, self.y)


# value y is not inversed yet
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


p16: Point = Point(0, 0, 0)
p18: Point = Point(0, 0, 0)
p20: Point = Point(0, 0, 0)
p22: Point = Point(0, 0, 0)


class Position():  # 向量与部位，存储了两个Point（这是可以的嘛）
    def __init__(self, e1, e2):
        self.e1 = e1
        self.e2 = e2

    def length_of_vector(self):
        return math.sqrt(math.pow(self.e1, 2) + math.pow(self.e2, 2))

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
    p19new = Point(1.5 * p19.x - 0.5 * p15.x, 1.5 * p19.y - 0.5 * p15.y, 1.5 * p19.z - 0.5 * p15.z)
    p17new = Point(1.5 * p17.x - 0.5 * p15.x, 1.5 * p17.y - 0.5 * p15.y, 1.5 * p17.z - 0.5 * p15.z)
    p21new = Point(1.5 * p21.x - 0.5 * p15.x, 1.5 * p21.y - 0.5 * p15.y, 1.5 * p21.z - 0.5 * p15.z)
    # 判断香蕉
    banana1 = banana(position.e1, position.e2, p15, p17new)
    banana2 = banana(position.e1, position.e2, p15, p19new)
    banana3 = banana(position.e1, position.e2, p15, p21new)
    return banana1 or banana2 or banana3


'''# get_right_index_v1 by possibility
def get_right_index(handedness_classification):
   
    :param handtrack_results:
    :return: right hand's index
   
    for idx, hand_handedness in enumerate(handedness_classification):
        list = MessageToDict(hand_handedness)["classification"]
        for hand in list:
            # when finally get the mirrored image,turn 'Left' to 'Right'
            if hand['label'] == 'Left' :
                print(hand['index'])
                return hand['index']
    return None
'''


# this fxxking slxt would never recognize left hand as the right one
def get_right_index(results, p15):
    index = 0
    myhand = results.multi_hand_landmarks
    h, w, c = img.shape
    dis = h * w
    index = 0
    for hand in myhand:
        p0_x = MessageToDict(hand.landmark[0])["x"] * w
        p0_y = MessageToDict(hand.landmark[0])["y"] * h
        distance = math.pow((p15.x - p0_x), 2) + math.pow((p15.y - p0_y), 2)
        if distance < 2000:
            idx = index
            return idx
        index += index
    return None


# from hand detector
def get_hand_bbox(results, idx):
    xList = []
    yList = []
    bbox = []
    bboxInfo = []
    handlmList = []
    myHand = results.multi_hand_landmarks[idx]
    for id,lm in enumerate(myHand.landmark):
        h, w, c = img.shape
        px, py = int(lm.x * w), int(lm.y * h)
        xList.append(px)
        yList.append(py)
        handlmList.append([px, py])

    xmin, xmax = min(xList), max(xList)
    ymin, ymax = min(yList), max(yList)
    boxW, boxH = xmax - xmin, ymax - ymin
    # bboxinsinfo = xmin, ymin, boxW, boxH
    #cx, cy = bboxinsinfo[0] + (bboxinsinfo[2] // 2), \
    #            bboxinsinfo[1] + (bboxinsinfo[3] // 2)
    # get four vertexes of bbox as a fxxking list
    return [(xmin, ymin), (xmin, ymax), (xmax, ymax), (xmax, ymin)]


# from pose detector
def get_hand_bbox2():
    # 延长手部线段
    p17new = Point(int(1.5 * p17.x - 0.5 * p15.x), int(1.5 * p17.y - 0.5 * p15.y), int(1.5 * p17.z - 0.5 * p15.z))
    p19new = Point(int(1.5 * p19.x - 0.5 * p15.x), int(1.5 * p19.y - 0.5 * p15.y), int(1.5 * p19.z - 0.5 * p15.z))
    draw_box([p17.get_xy(), p17new.get_xy(), p19new.get_xy(), p19.get_xy()], (0, 100, 0))
    return [p17.get_xy(), p17new.get_xy(), p19new.get_xy(), p19.get_xy()]


def three_equal_point(p1, p2):
    '''

    :param p1: Point
    :param p2: Point
    :return: p3,p4 (p1,p3,p4,p2)
    '''
    p3: Point = Point(0, 0, 0)
    p4: Point = Point(0, 0, 0)
    p3.x = int((2 * p1.x + p2.x) / 3)
    p3.y = int((2 * p1.y + p2.y) / 3)
    p4.x = int((2 * p2.x + p1.x) / 3)
    p4.y = int((2 * p2.y + p1.y) / 3)
    return p3, p4


def half_point(p1, p2):
    '''

    :param p1: Point
    :param p2: Point
    :return: p3,p4 (p1,p3,p2)
    '''
    p3: Point = Point(0, 0, 0)
    p3.x = int((p1.x + p2.x) / 2)
    p3.y = int((p1.y + p2.y) / 2)
    return p3


def intersection_area(poly1, poly2):
    if poly1.intersection(poly2):
        return poly1.intersection(poly2).area
    return 0


def draw_box(vertexlist, color):
    thickness = 5
    cv2.line(img, vertexlist[0], vertexlist[1], color, thickness)
    cv2.line(img, vertexlist[1], vertexlist[2], color, thickness)
    cv2.line(img, vertexlist[2], vertexlist[3], color, thickness)
    cv2.line(img, vertexlist[3], vertexlist[0], color, thickness)


while True:
    success, img = capture.read()
    if success:


        # mirror the image
        img = cv2.flip(img, flipCode=1)
        img = poseDetector.findPose(img)
        lmList, bboxInfo = poseDetector.findPosition(img, draw=False)
        imgRGB = cv2.cvtColor(img, cv2.COLOR_BGR2RGB)
        results = hands.process(imgRGB)
        if bboxInfo:
            p11 = DirectPoint(lmList, 11, img.shape[0])
            p12 = DirectPoint(lmList, 12, img.shape[0])
            p14 = DirectPoint(lmList, 14, img.shape[0])
            p16 = DirectPoint(lmList, 16, img.shape[0])
            p23 = DirectPoint(lmList, 23, img.shape[0])
            p24 = DirectPoint(lmList, 24, img.shape[0])
            p25 = DirectPoint(lmList, 25, img.shape[0])
            p26 = DirectPoint(lmList, 26, img.shape[0])
            # 初始化了五个部位对应的lm
            # [0]左上臂11,13;[1]左下臂13,15;[2]左大腿23,25;[3]右大腿24,26;[4]心脏*
            positionList = [Position(p12, p14), Position(p14, p16),
                            Position(p23, p25), Position(p24, p26)]

            # 初始化手部四个lm
            p15 = DirectPoint(lmList, 15, img.shape[0])
            p17 = DirectPoint(lmList, 17, img.shape[0])
            p19 = DirectPoint(lmList, 19, img.shape[0])
            p21 = DirectPoint(lmList, 21, img.shape[0])

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
            # per frame get where the right hand is(position)
            data = np.array([position])

            # test arm box
            # draw_box(positionList[0].get_arm_box(),(0,0,100))

            '''
            # Op.1 Hand Detector's bbox
            # determine the index of right hand in current frame
            right_index = results.multi_handedness
            if right_index:
                right_index = get_right_index(results, p16)
            else:
                right_index = None
            lm = results.multi_hand_landmarks
            if lm and right_index != None:
                # get the bbox of right hand in current frame
                bbox = get_hand_bbox(results, right_index)

                # get the fxxking 6 areas in body
                #  firstly get the bull-shit added points
                p34, p36 = three_equal_point(p12, p24)
                p33, p35 = three_equal_point(p11, p23)
                p37 = half_point(p11, p12)
                p38 = half_point(p34, p33)
                p39 = half_point(p36, p35)
                p40 = half_point(p23, p24)
                #  initialize a poly item
                handBboxPoly = Polygon(bbox)
                testPoly20 = Polygon([p37.get_xy(), p11.get_xy(), p33.get_xy(), p38.get_xy()])
                testPoly21 = Polygon([p37.get_xy(), p12.get_xy(), p34.get_xy(), p38.get_xy()])
                testPoly22 = Polygon([p38.get_xy(), p33.get_xy(), p35.get_xy(), p39.get_xy()])
                testPoly23 = Polygon([p38.get_xy(), p34.get_xy(), p36.get_xy(), p39.get_xy()])
                testPoly24 = Polygon([p39.get_xy(), p35.get_xy(), p23.get_xy(), p40.get_xy()])
                testPoly25 = Polygon([p39.get_xy(), p40.get_xy(), p24.get_xy(), p36.get_xy()])
                bodyPoly = Polygon([p12.get_xy(), p11.get_xy(), p23.get_xy(), p24.get_xy()])
                '''
            p34, p36 = three_equal_point(p12, p24)
            p33, p35 = three_equal_point(p11, p23)
            p37 = half_point(p11, p12)
            p38 = half_point(p34, p33)
            p39 = half_point(p36, p35)
            p40 = half_point(p23, p24)
            #  initialize a poly item
            handBboxPoly2 = Polygon(get_hand_bbox2())
            # body polygons
            testPoly20 = Polygon([p37.get_xy(), p11.get_xy(), p33.get_xy(), p38.get_xy()])
            testPoly21 = Polygon([p37.get_xy(), p12.get_xy(), p34.get_xy(), p38.get_xy()])
            testPoly22 = Polygon([p38.get_xy(), p33.get_xy(), p35.get_xy(), p39.get_xy()])
            testPoly23 = Polygon([p38.get_xy(), p34.get_xy(), p36.get_xy(), p39.get_xy()])
            testPoly24 = Polygon([p39.get_xy(), p35.get_xy(), p23.get_xy(), p40.get_xy()])
            testPoly25 = Polygon([p39.get_xy(), p40.get_xy(), p24.get_xy(), p36.get_xy()])
            bodyPoly = Polygon([p12.get_xy(), p11.get_xy(), p23.get_xy(), p24.get_xy()])
            # arms polygons
            s0_vertex = positionList[0].get_arm_box()
            s1_vertex = positionList[1].get_arm_box()
            s2_vertex = positionList[2].get_arm_box()
            s3_vertex = positionList[3].get_arm_box()
            Poly0 = Polygon(s0_vertex)
            Poly1 = Polygon(s1_vertex)
            Poly2 = Polygon(s2_vertex)
            Poly3 = Polygon(s3_vertex)
            # intersection with arms
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
            # hand inside body
            if bodyPoly.intersects(handBboxPoly2):

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
            if s1!=0 or s2!=0 or s3!= 0 or s0!=0:
                position = max(dict_position, key= lambda x: x['area'])['position']
                draw_box(max(dict_position, key= lambda x: x['area'])['vertex'], (255, 0, 0))
                data = np.array([position])

                # draw the right hand's lm
                # mpDraw.draw_landmarks(img, lm[right_index], mpHands.HAND_CONNECTIONS)

            print(data)
            sock.sendto(str.encode(str(data)), serverAddressPort) #send info to unity

    cv2.imshow("image", img)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break
capture.release()
cv2.destroyAllWindows()