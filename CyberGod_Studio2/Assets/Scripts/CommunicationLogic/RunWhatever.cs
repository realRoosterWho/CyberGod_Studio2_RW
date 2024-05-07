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


public class RunWhatever : MonosingletonTemp<RunWhatever>
{
    private ProcessStartInfo startInfo;
    private Process process;

    private UdpClient udpClient;
    private IPEndPoint remoteEP;


    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellExecute(IntPtr hwnd, string lpszOp, string lpszFile, string lpszParams, string lpszDir, int FsShowCmd);



    void Start()
    {
        StartProcess();

    }

    private void StartProcess()
    {
        Kill_All_Python_Process();

        udpClient = new UdpClient();
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5005);
        string fullPath = "";



        // deal with the motherfxxking file path issue on different systems
        // on wins
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // no window
            string exePath = "Scripts/bodydivide_noimage_v30504/dist/main/main.exe";

            // has windos
            // string exePath = "Scripts/bodydivide_onimage_v30504/dist/main/main.exe";

            string dataPath = Application.dataPath;
            fullPath = dataPath + "/" + exePath;

            startInfo = new ProcessStartInfo();
            startInfo.FileName = fullPath;

            process = new Process();
            process.StartInfo = startInfo;

            process.Start();


            // ShellExecute(IntPtr.Zero, "open", fullPath, "", "", 1);
        }

        // on mac runpy
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {

            fullPath = "/Volumes/Rooster_SSD/_Unity_Projects/CyberGod_Studio2/CyberGod_Studio2_RW/CyberGod_Studio2/Assets/Scripts/bodydivide_test_v20402/main.py";
            startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            string command = "source activate cybergod; python \"" + fullPath + "\"";
            //command һ�������õ���䣬�������ն������е���䣬��conda��pythonû��ϵ
            // string commandtest = "source activate cybergod"; 

            // startInfo.FileName = "/bin/bash";
            startInfo.FileName = "/bin/zsh";
            startInfo.Arguments = "-i -l -c \"" + command + "\"";

            process = new Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);

            process.Start();
            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

        }

    }
    
    private void Update()
    {
        
        //检查进程是否存在，如果不存在就重新启动进程
        if (process.HasExited)
        {
            StartProcess();
        }
        

    }


    private IEnumerator RestartAfterDelay()
    {
        // 等待两秒
        yield return new WaitForSeconds(2);

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
            StartCoroutine(RestartAfterDelay());
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
                if ((processName.ToLower().Contains("python") || processName.ToLower().Contains("main")) && process_1.Id != Process.GetCurrentProcess().Id)
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
        Kill_All_Python_Process();
        UnityEngine.Debug.Log("Quit");
    }


}
