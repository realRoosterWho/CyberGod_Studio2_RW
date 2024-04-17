
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

public class U2PThread : MonoBehaviour
{
    Thread receiveThread;
   	UdpClient client;
    public int port = 5005;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;

    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (startRecieving)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

              
                //现在data看起来像是[99.          1.          0.          1.          0.6984375   0.27083333]，去除中括号成为字符串
                data = data.Replace("[", "");
                data = data.Replace("]", "");

                string[] newdata = Regex.Split(data, "\\s+", RegexOptions.IgnoreCase);

                float bbox_on = float.Parse(newdata[0]);   //是否正常动捕，为0时后续变量无效
                float handPosData = float.Parse(newdata[1]);   //右手所处部位编号，默认99
                float kneeIn = float.Parse(newdata[2]);     //膝盖处标记点是否在画面内：是-1，否-0
                float handIn = float.Parse(newdata[3]);     //右手是否在画面内：是-1，否-0
                float handX = float.Parse(newdata[4]);      //右手相对横坐标，需要乘以光标移动范围宽度食用
                float handY = float.Parse(newdata[5]);      //右手相对纵坐标，需要乘以光标移动范围长度食用

                /*
				data = data.Replace("[", "");
				data = data.Replace("]", "");
				float dataFloat = float.Parse(data);
                */

				SendData(handPosData);

                //DebugNewData
    //             Debug.Log(data);
				// Debug.Log("bbox_on: " + bbox_on);
				Debug.Log("handPosData: " + handPosData);
				// Debug.Log("kneeIn: " + kneeIn);
				// Debug.Log("handIn: " + handIn);
				// Debug.Log("handX: " + handX);
				// Debug.Log("handY: " + handY);

            }
			catch (Exception err)
            {
        		print(err.ToString()); 
            }
        }
    }

	private void SendData(float data)
    {
		//Debug.Log(data);
		GameEventArgs args = new GameEventArgs {	FloatValue = data	};
		EventManager.Instance.TriggerEvent("MotionCaptureInput", args);
	}


}