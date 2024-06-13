using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_CameraReady : MonoBehaviour
{
    
    //获取TextMeshProUGUI组件
    public TextMeshProUGUI textMeshProUGUI;
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
            textMeshProUGUI.text = "摄像头正在启动……";
        }
        else if (CameraReadyReader.Instance.data == 99)
        {
            textMeshProUGUI.text = "摄像头已启动。将手臂置于胸前检查距离是否合适。";
        }
        else
        {
            textMeshProUGUI.text = "距离合适";
        }

    }
}
