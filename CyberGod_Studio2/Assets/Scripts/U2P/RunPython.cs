using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEditor;
using System.Runtime.InteropServices;


public class RunPython : MonoBehaviour
{
    private ProcessStartInfo startInfo;
    private Process process;

    private UdpClient udpClient;
    private IPEndPoint remoteEP;
    
    private string command;

    void Start()
    {
        Kill_All_Python_Process();

        udpClient = new UdpClient();
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5005);
        // 这是python文件的路径 this is python file path
        string pythonPath = "communication_test0327/main.py";
        string dataPath = Application.dataPath;
        string fullPath = dataPath + "/" + pythonPath;
        // 这是python文件的命令 this is python file command


        startInfo = new ProcessStartInfo();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            startInfo.FileName = "cmd.exe";
            command = "/c activate base & python \"" + fullPath + "\"";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            startInfo.FileName = "/bin/bash";
            command = "-c \"source activate base && python '" + fullPath + "'\"";
        }

        startInfo.Arguments = command;
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false; 
        startInfo.RedirectStandardOutput = true; //重定向标准输出,目的是让python的print函数输出到Unity的Console面板
        startInfo.RedirectStandardError = true;

        process = new Process();
        process.StartInfo = startInfo;
        process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
        process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);

        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            UnityEngine.Debug.Log(e.Data);
            if (e.Data == "StartRecognition")
            {
                print("Recognizing");
            }
        }
    }
    private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Data))
        {
            UnityEngine.Debug.LogError("Received error output: " + e.Data);
        }
    }

    void Kill_All_Python_Process()
    {
        Process[] allProcesses = Process.GetProcesses();
        foreach (Process process_1 in allProcesses)
        {
            try
            {
                string processName = process_1.ProcessName;
                if (processName.ToLower().Contains("python") && process_1.Id != Process.GetCurrentProcess().Id)
                {
                    process_1.Kill();
                }
            }
            catch (Exception ex)
            {
                print(ex);
            }
        }
    }
    void OnApplicationQuit()
    {
        UnityEngine.Debug.Log("Application ending after " + Time.time + " seconds");
        Kill_All_Python_Process();
    }
}
