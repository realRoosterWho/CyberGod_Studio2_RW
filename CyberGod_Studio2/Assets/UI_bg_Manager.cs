using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间

public class UI_bg_Manager : MonoBehaviour
{
    public List<Sprite> sprites; // Sprite列表
    private Image image; // Image组件

    // Start is called before the first frame update
    void Start()
    {
        // 获取Image组件
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        // 检查ControlMode_Manager的m_controlMode
        if (ControlMode_Manager.Instance.m_controlMode == ControlMode.REPAIRING)
        {
            // 如果m_controlMode是REPAIRING，使用贴图【1】
            image.sprite = sprites[1];
        }
        else
        {
            // 否则，使用贴图【0】
            image.sprite = sprites[0];
        }
    }
}