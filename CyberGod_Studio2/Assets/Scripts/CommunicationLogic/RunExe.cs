using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class RunExe : MonoBehaviour
{
    public InputField inputField;
    public Button ButtonRunExe;

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    public static extern int ShellExecute(IntPtr hwnd, string lpszOp, string lpszFile, string lpszParams, string lpszDir, int FsShowCmd);

    void Start()
    {
        ButtonRunExe.onClick.AddListener(() =>
        {
            var exePath = "E:\\cg\\communication_capture0329\\dist\\main\\main.exe";
            // var dir = "E:\\cg\\";
            ShellExecute(IntPtr.Zero, "open", exePath, "", "", 1);
        });
    }

}
