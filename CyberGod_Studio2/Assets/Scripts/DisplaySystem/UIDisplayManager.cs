using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIDisplayManager : MonosingletonTemp<UIDisplayManager>
{   
    
    [Header("LeftDisplay")]

    [SerializeField] private Image m_leftimage;
    [SerializeField] public TextMeshProUGUI m_leftTitle;
    [SerializeField] public TextMeshProUGUI m_leftDescription;
    
    [Space(10)] // 添加 10 像素的间隔
    
    
    
    [Header("SpiritSpeakDisplay")]
    [SerializeField] public TextMeshProUGUI m_spiritSpeak;
    [SerializeField] public Image m_spiritImage;
    [Space(10)] // 添加 10 像素的间隔
    
    
    
    [Header("ScoreDisplay")]
    [SerializeField] public TextMeshProUGUI m_scoreDisplay;

    [Space(10)] // 添加 10 像素的间隔

    [Header("IntroOutroDisplay")]
    [SerializeField] public GameObject m_introOutroGroup;
    [SerializeField] public TextMeshProUGUI m_introOutroText;
    [SerializeField] public Image m_introOutroImage;


    private bool isDisplayCalled = false;
    private bool isSpiritSpeakCalled = false;
    
    private bool isIntroOutroCalled = false;
    
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
        
        if (!isIntroOutroCalled)
        {
            // 如果DisplayIntroOutro没有被调用，就清空显示
            m_introOutroText.text = "";
            m_introOutroImage.sprite = null;
            m_introOutroGroup.SetActive(false);
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
    
    public void DisplayIntroOutro(SpiritSpeakEntry entry)
    {
        m_introOutroGroup.SetActive(true);
        m_introOutroText.text = entry.dialogueText;
        m_introOutroImage.sprite = entry.SpiritImage;
        
        // 标记DisplayIntroOutro已被调用
        isIntroOutroCalled = true;
    }
    
    public void DisplayScore(int errorNumber, int maxErrorNumber)
    {
        m_scoreDisplay.text = errorNumber + "/" + maxErrorNumber;
    }
}