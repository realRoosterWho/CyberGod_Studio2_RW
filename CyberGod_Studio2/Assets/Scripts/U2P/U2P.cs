using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class U2P
{
    private static U2P instance;
    private Socket serverSocket;
    private Socket clientSocket;
    public bool isConnected;

    public static U2P Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new U2P();
            }
            return instance;
        }
    }
    private U2P()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        serverSocket.Bind(new IPEndPoint(ipAddress, 5005));
        serverSocket.Listen(5);
        StartServer();
    }
    private void StartServer()
    {
        serverSocket.BeginAccept(AcceptCallback, null);
    }
    private void AcceptCallback(IAsyncResult ar)
    {
        Socket sc = serverSocket.EndAccept(ar);
        if (clientSocket == null)
        {
            clientSocket = sc;
            isConnected = true;
            Debug.Log("���ӳɹ�");
        }
    }
    public float[] RecData()
    {
        try
        {
            if (clientSocket != null)
            {
                int canRead = clientSocket.Available;
                byte[] buff = new byte[canRead];
                clientSocket.Receive(buff);
                string str = Encoding.UTF8.GetString(buff);
                if (str == "")
                {
                    return null;
                }
                string[] strData = str.Split(',');
                float[] data = new float[strData.Length];
                for (int i = 0; i < strData.Length; i++)
                {
                    data[i] = float.Parse(strData[i]);
                }
                return data;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("��������:" + ex);
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
                isConnected = false;
                StartServer();
            }
        }
        return null;
    }
    public void SendData(List<float> data)
    {
        try
        {
            if (clientSocket != null)
            {
                string strData = "";
                for (int i = 0; i < data.Count; i++)
                {
                    strData += data[i];
                    if (i != data.Count - 1)
                    {
                        strData += ",";
                    }
                }
                byte[] dataBytes = Encoding.UTF8.GetBytes(strData);
                clientSocket.Send(dataBytes);
            }
        }
        catch (Exception ex)
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
                isConnected = false;
                StartServer();
            }
        }
    }

}