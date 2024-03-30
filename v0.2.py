import cv2
import cvzone
import numpy as np
from cvzone.HandTrackingModule import HandDetector
from cvzone.PoseModule import PoseDetector
import os


capture = cv2.VideoCapture(0)
# 尚未镜像，看到的是右手就是右手
poseDetector = PoseDetector()
handDetector = HandDetector(maxHands=2, detectionCon=0.8)
posList = []


# 这边想写一个继承的关系
# 父类Point通过三个坐标直接初始化
# 子类DirectPoint相当于直接从动捕数据提取坐标，需要传入当前帧的lmList和img.shape()
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
def hand_touch(A, B):
    banana1 = banana(A, B, p16, p18)
    banana2 = banana(A, B, p16, p20)
    banana3 = banana(A, B, p16, p22)
    return banana1 or banana2 or banana3


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
            # 五个部位[0]左上臂11,13 [1]左下臂13,15 [2]左大腿23,25 [3]右大腿24,26 [4]心脏*
            positionList = [[p11, p13], [p13, p15], [p23, p25], [p24, p26]]

            # 手部四个lm
            p16 = DirectPoint(lmList, 16, img.shape[0])
            p18 = DirectPoint(lmList, 18, img.shape[0])
            p20 = DirectPoint(lmList, 20, img.shape[0])
            p22 = DirectPoint(lmList, 22, img.shape[0])

            # 逐帧手部触碰判定
            if hand_touch(positionList[0][0], positionList[0][1]):
                print('touch!')


    cv2.imshow("image", img)
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

capture.release()
cv2.destroyAllWindows()