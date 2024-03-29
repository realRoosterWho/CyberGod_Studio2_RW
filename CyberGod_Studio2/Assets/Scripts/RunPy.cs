using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System;
using UnityEditor;

public class RunPy : MonoBehaviour
{
    private ProcessStartInfo startInfo;
    private Process process;

    private UdpClient udpClient;
    private IPEndPoint remoteEP;

    void Start()
    {
        Kill_All_Python_Process();

        udpClient = new UdpClient();
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5005);
        // python项目在unity项目中的相对路径（从Assets文件夹开始）
        string pythonPath = "/main.py";
        string dataPath = Application.dataPath;
        string fullPath = dataPath + "/" + pythonPath;
        //  "base" 换成虚拟环境名称（root？）
        string command = "/c activate base & python \"" + fullPath + "\"";

        startInfo = new ProcessStartInfo();

        // 执行cmd
        startInfo.FileName = "cmd.exe";
        // 输入参数是上一步的command字符串
        startInfo.Arguments = command;
        // 不显示cmd窗口
        startInfo.CreateNoWindow = true;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;
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
        UnityEngine.Debug.Log("应用程序即将退出，清理所有Python进程");
        Kill_All_Python_Process();
    }
}
