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
        string fullPath = "";

        // deal with the motherfxxking file path issue on different systems
        // on wins
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string pythonPath = "Scripts/communication_capture0329/main.py";
            string dataPath = Application.dataPath;
            fullPath = dataPath + "/" + pythonPath;
        }
        // on mac
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            fullPath = "/Volumes/Rooster_SSD/_Unity_Projects/CyberGod_Studio2/CyberGod_Studio2_RW/CyberGod_Studio2/Assets/Scripts/communication_capture0329/main.py";
        }

        startInfo = new ProcessStartInfo();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            string command = "/c activate base & python \"" + fullPath + "\"";
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            string command = "source activate cybergod; python \"" + fullPath + "\"";
            startInfo.FileName = "/bin/bash";
            startInfo.Arguments = "-c \"" + command + "\"";
        }

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
        UnityEngine.Debug.Log("Quit");
        Kill_All_Python_Process();
    }
}
