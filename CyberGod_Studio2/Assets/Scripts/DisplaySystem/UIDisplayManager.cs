using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplayManager : MonosingletonTemp<UIDisplayManager>
{   
    [SerializeField] private Image m_leftimage;
    [SerializeField] public TextMeshProUGUI m_leftTitle;
    [SerializeField] public TextMeshProUGUI m_leftDescription;

    private bool isDisplayCalled = false;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (!isDisplayCalled)
        {
            // 如果DisplayInfo没有被调用，就清空显示
            m_leftimage.sprite = null;
            m_leftTitle.text = "";
            m_leftDescription.text = "";
            m_leftimage.enabled = false;

        }
        // 重置isDisplayCalled为false
        isDisplayCalled = false;
    }
    
    public void DisplayLeftInfo(ObjectInfo info)
    {
        if (info == null)
        {
            return;
        }
        if (info.image == null)
        {
            // 如果info.image为null，隐藏Image组件
            m_leftimage.enabled = false;
        }
        else
        {
            // 如果info.image不为null，显示Image组件并设置Sprite
            m_leftimage.enabled = true;
            m_leftimage.sprite = info.image;
        }
        m_leftTitle.text = info.name;
        m_leftDescription.text = info.description;
        // 标记DisplayInfo已被调用
        isDisplayCalled = true;
    }
}