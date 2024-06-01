using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPointLogic : MonoBehaviour
{
    public Sprite sprite1; // 第一个精灵
    public Sprite sprite2; // 第二个精灵

    private Image image; // Image组件

    // Start is called before the first frame update
    void Start()
    {
        // 获取Image组件
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("没有找到Image组件！");
        }
        // 使用第一个精灵
        SwitchSprite(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 切换精灵的函数
    public void SwitchSprite(bool useSprite1)
    {
        if (image != null)
        {
            image.sprite = useSprite1 ? sprite1 : sprite2;
        }
    }

    // 关闭Image组件的函数
    public void DisableImage()
    {
        if (image != null)
        {
            image.enabled = false;
        }
    }
}