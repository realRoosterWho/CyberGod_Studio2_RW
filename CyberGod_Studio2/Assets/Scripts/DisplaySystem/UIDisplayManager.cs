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
    
    
    [SerializeField] public TextMeshProUGUI m_spiritSpeak;
    [SerializeField] public Image m_spiritImage;
    
    [SerializeField] public TextMeshProUGUI m_scoreDisplay;

    private bool isDisplayCalled = false;
    private bool isSpiritSpeakCalled = false;
    
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
        
        if (!isSpiritSpeakCalled)
        {
            // 如果DisplaySpiritSpeak没有被调用，就清空显示
            m_spiritSpeak.text = "";
            m_spiritImage.sprite = null;
            m_spiritImage.enabled = false;
        }
        
        
        
        
        
        // 重置isDisplayCalled为false
        isDisplayCalled = false;
        // 重置isSpiritSpeakCalled为false
        isSpiritSpeakCalled = false;
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

    public void DisplaySpiritSpeak(SpiritSpeakEntry entry)
    {
        m_spiritImage.enabled = true;
        m_spiritSpeak.text = entry.dialogueText;
        m_spiritImage.sprite = entry.SpiritImage;
        
        // 标记DisplaySpiritSpeak已被调用
        isSpiritSpeakCalled = true;
    }
    
    public void DisplayScore(int errorNumber, int maxErrorNumber)
    {
        m_scoreDisplay.text = errorNumber + "/" + maxErrorNumber;
    }
}