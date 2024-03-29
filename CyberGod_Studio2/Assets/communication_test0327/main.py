import socket
import numpy as np


host = '127.0.0.1'
port = 5005


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


# 生成一个[1,10]的随机数组发送给unity
'''
data = np.random.randint(2,10,size = [1,10])
'''
data = np.array([1])
connect_unity(host, port)
send_to_unity(data)
rec_from_unity()











