using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




public class U2PThread : MonosingletonTemp<U2PThread>
{
    Thread receiveThread;
    UdpClient receiveClient;
    UdpClient sendClient;
    public int sendPort = 5005; // 发送数据到Python的端口
    public int receivePort = 5006; // 接收来自Python数据的端口
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;

    public bool showCameraDropdown = true; // 控制是否显示下拉菜单
    public TMP_Dropdown cameraDropdown; // TextMeshPro的Dropdown引用
    

    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData)); // 创建一个新的线程，然后调用ReceiveData函数
        receiveThread.IsBackground = true;
        receiveThread.Start();

        sendClient = new UdpClient(); // 创建发送用的UDP客户端

        if (showCameraDropdown && cameraDropdown != null)
        {
            // 初始设置下拉菜单，显示“稍等，获取摄像头列表中...”
            cameraDropdown.ClearOptions();
            cameraDropdown.AddOptions(new List<string> { "稍等，获取摄像头列表中..." });
            
            //InitializeCameraDropdown(); // 如果需要，初始化下拉菜单
            StartCoroutine(InitializeCameraDropdown());
        }
    }

    private IEnumerator InitializeCameraDropdown()
    {
        // 获取摄像头名称和最大分辨率列表
        List<(string name, int width, int height)> cameraInfos = new List<(string, int, int)>();

        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            var device = WebCamTexture.devices[i];
            WebCamTexture webcam = new WebCamTexture(device.name);

            // 启动摄像头以获取其分辨率
            webcam.Play();

            // 等待摄像头初始化
            yield return new WaitForSeconds(1f);

            int width = webcam.width;
            int height = webcam.height;

            // 如果分辨率信息尚未加载完全，等待一小段时间
            while (width <= 16 || height <= 16)
            {
                yield return new WaitForSeconds(0.1f);
                width = webcam.width;
                height = webcam.height;
                Debug.Log("Waiting for camera resolution...");
                Debug.Log($"Width: {width}, Height: {height}");
            }

            webcam.Stop();

            cameraInfos.Add((device.name, width, height));
        }

        // 对摄像头信息按分辨率降序排序
        cameraInfos = cameraInfos.OrderByDescending(c => c.width * c.height).ToList();

        // 构建下拉菜单选项列表
        List<string> cameraOptions = new List<string> { "请选择摄像头" };
        cameraOptions.AddRange(cameraInfos.Select(c => $"({c.width}x{c.height}) {c.name}"));

        // 更新下拉菜单选项
        cameraDropdown.ClearOptions();
        cameraDropdown.AddOptions(cameraOptions);

        cameraDropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(cameraDropdown);
        });
    }

    void DropdownValueChanged(TMP_Dropdown change)
    {
        string selectedCamera = change.options[change.value].text;
        Debug.Log("Selected camera: " + selectedCamera);

        // 如果用户选择了“请选择摄像头”，则不发送消息
        if (change.value == 0)
        {
            return;
        }

        // 发送摄像头索引到Python脚本
        int cameraIndex = change.value - 1; // 因为第一个选项是“请选择摄像头”，实际索引需要减1
        SendMessageToPython(cameraIndex.ToString());
    }

    void SendMessageToPython(string message)
    {
        byte[] data = Encoding.UTF8.GetBytes(message);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), sendPort);
        sendClient.Send(data, data.Length, endPoint); // 使用发送用的UDP客户端发送数据
        Debug.Log("Sent message to Python: " + message);
    }

    private void ReceiveData()
    {
        try
        {
            // 创建UdpClient并设置SO_REUSEADDR选项
            receiveClient = new UdpClient();
            receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            receiveClient.Client.Bind(new IPEndPoint(IPAddress.Any, receivePort));

            Debug.Log("Started receiving data...");

            while (startRecieving)
            {
                try
                {
                    IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                    Debug.Log("Waiting for data...");
                    byte[] dataByte = receiveClient.Receive(ref anyIP);
                    Debug.Log("Data received.");
                    data = Encoding.UTF8.GetString(dataByte);
                    Debug.Log("Received message from Python: " + data);

                    // 处理接收到的数据
                    data = data.Replace("[", "");
                    data = data.Replace("]", "");

                    string[] newdata = Regex.Split(data, "\\s+", RegexOptions.IgnoreCase);
                    if (newdata[0] == "")
                    {
                        newdata[0] = newdata[1];
                        newdata[1] = newdata[2];
                        newdata[2] = newdata[3];
                        newdata[3] = newdata[4];
                        newdata[4] = newdata[5];
                        newdata[5] = newdata[6];
                    }

                    float bbox_on = float.Parse(newdata[0]);   // 是否正常动捕，为0时后续变量无效
                    float handPosData = float.Parse(newdata[1]);   // 右手所处部位编号，默认99
                    float kneeIn = float.Parse(newdata[2]);     // 膝盖处标记点是否在画面内：是-1，否-0
                    float handIn = float.Parse(newdata[3]);     // 右手是否在画面内：是-1，否-0
                    float handX = float.Parse(newdata[4]);      // 右手相对横坐标，需要乘以光标移动范围宽度食用
                    float handY = float.Parse(newdata[5]);      // 右手相对纵坐标，需要乘以光标移动范围长度食用

                    SendData(handPosData);
                }
                catch (Exception err)
                {
                    Debug.LogError("Error receiving data: " + err.ToString());
                }
            }
        }
        catch (SocketException ex)
        {
            Debug.LogError("SocketException: " + ex.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.ToString());
        }
        finally
        {
            // 确保资源被正确释放
            if (receiveClient != null)
            {
                receiveClient.Close();
            }
        }
    }

    private void SendData(float data)
    {
        GameEventArgs args = new GameEventArgs { FloatValue = data };
        EventManager.Instance.TriggerEvent("MotionCaptureInput", args);
    }

    void OnApplicationQuit()
    {
        startRecieving = false;
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        if (receiveClient != null)
        {
            receiveClient.Close();
        }
        if (sendClient != null)
        {
            sendClient.Close();
        }
    }
}