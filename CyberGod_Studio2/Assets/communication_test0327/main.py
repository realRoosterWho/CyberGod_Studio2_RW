<<<<<<< Updated upstream
import socket
import numpy as np


host = '127.0.0.1'
port = 5005

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = (host, port)

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

data = np.random.randint(2,10,size = [1,10])

data = np.array([1])
# 通讯极简版
connect_unity(host, port)
send_to_unity(data)
rec_from_unity()
# cv2.imshow("image", img)











=======
# 这是一个示例 Python 脚本。

# 按 Shift+F10 执行或将其替换为您的代码。
# 按 双击 Shift 在所有地方搜索类、文件、工具窗口、操作和设置。


def print_hi(name):
    # 在下面的代码行中使用断点来调试脚本。
    print(f'Hi, {name}')  # 按 Ctrl+F8 切换断点。


# 按间距中的绿色按钮以运行脚本。
if __name__ == '__main__':
    print_hi('PyCharm')

# 访问 https://www.jetbrains.com/help/pycharm/ 获取 PyCharm 帮助
>>>>>>> Stashed changes
