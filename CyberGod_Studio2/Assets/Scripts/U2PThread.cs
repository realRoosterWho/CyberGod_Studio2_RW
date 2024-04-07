
using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

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
				
				//现在data看起来像是[99]，去除中括号成为浮点
				data = data.Replace("[", "");
				data = data.Replace("]", "");
				float dataFloat = float.Parse(data);
				SendData(dataFloat);
				
				//Debug
				//Debug.Log(data);
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