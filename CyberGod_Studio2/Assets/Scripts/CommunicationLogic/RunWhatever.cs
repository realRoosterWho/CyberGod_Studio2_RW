using System.IO;
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
using Debug = UnityEngine.Debug;

// Rest of your code...


public class RunWhatever : MonosingletonTemp<RunWhatever>
{
    private ProcessStartInfo startInfo;
    private Process process;

    private UdpClient udpClient;
    private IPEndPoint remoteEP;
    private int processId;
    
    private float checkInterval = 1f; // 每2秒检查一次
    private float nextCheckTime = 0f;


    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellExecute(IntPtr hwnd, string lpszOp, string lpszFile, string lpszParams, string lpszDir, int FsShowCmd);



    void Start()
    {
        StartProcess();

    }

    private void StartProcess()
    {
        Kill_Process();
        udpClient = new UdpClient();
        remoteEP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5005);
        string fullPath = "";



        // deal with the motherfxxking file path issue on different systems
        // on wins
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // no window

            // Assets/StreamingAssets/PythonApp/DSDSmain/DSDSmain.exe
            // System.IO.Path.Combine(Application.streamingAssetsPath, "PythonApp/DSDSmain/DSDSmain.exe")
            string exePath = "winpython/main.exe";
            // has windos
            // string exePath = "Scripts/bodydivide_onimage_v30504/dist/main/main.exe";

            string dataPath = Application.streamingAssetsPath;
            fullPath = dataPath + "/" + exePath;

            startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;//不显示程序窗口
            startInfo.UseShellExecute = false;//是否使用操作系统shell启动
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            
            startInfo.FileName = fullPath;

            process = new Process();
            process.StartInfo = startInfo;

            process.Start();
            //Debug,但是指明命名空间
            UnityEngine.Debug.Log("Starting process: " + fullPath);
            processId = process.Id;



            //ShellExecute(IntPtr.Zero, "open", fullPath, "", "", 1);
        }

        // on mac runpy
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            string relativePath = "buildforMac/motionCaptureforMac";
            fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);
            startInfo = new ProcessStartInfo();

            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            startInfo.FileName = fullPath;

            process = new Process();
            process.StartInfo = startInfo;
            process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
            process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);
            UnityEngine.Debug.Log("Starting process: " + fullPath);

            process.Start();
            processId = process.Id;

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
        }

    }
    
    private void Update()
    {
        if (Time.time > nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            if (process.HasExited)
            {
                UnityEngine.Debug.LogError("Process has exited");
                StartCoroutine(RestartAfterDelay());
            }
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
            UnityEngine.Debug.Log("Received error output: " + e.Data);
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
    
    void Kill_Process()
    {
        try
        {
            var process = Process.GetProcessById(processId);
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }
        catch (Exception ex)
        {
            print(ex);
        }
    }

    void OnApplicationQuit()
    {
        Kill_Process();
        UnityEngine.Debug.Log("Quit");
    }


}
