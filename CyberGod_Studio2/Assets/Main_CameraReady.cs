using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_CameraReady : MonoBehaviour
{
    
    public main_bg_Logic m_main_bg_Logic;
    //获取TextMeshProUGUI组件
    public TextMeshProUGUI textMeshProUGUI;
    public TextMeshProUGUI textMeshProUGUI2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //监听CameraReadyReader的data
        if (CameraReadyReader.Instance.data == 0)
        {
            textMeshProUGUI.text = "摄像头正在初始化,请选择摄像头……(若第一次启动，给予权限后需要重启软件)";
        }
        else if (CameraReadyReader.Instance.data == 99)
        {
            textMeshProUGUI.text = "摄像头已启动。将手臂置于胸前检查距离是否合适。";
        }
        else
        {
            textMeshProUGUI.text = "距离合适";
            m_main_bg_Logic.canJump = true;
            textMeshProUGUI2.enabled = true;
        }

    }
}
