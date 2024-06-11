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

    [Header("IntroDisplay")]
    [SerializeField] public GameObject m_introGroup;
    [SerializeField] public TextMeshProUGUI m_introText;
    [SerializeField] public Image m_introImage;
    [Space(10)] // 添加 10 像素的间隔

    
    
    
    [Header("OutroDisplay")]
    [SerializeField] public GameObject m_outroGroup;
    [SerializeField] public TextMeshProUGUI m_outroText;
    [SerializeField] public Image m_outroImage;
    [Space(10)] // 添加 10 像素的间隔

    
    
    
    [Header("SpecialDisplay")] // 新增
    [SerializeField] public Image m_specialImage; // 新增


    private bool isDisplayCalled = false;
    private bool isSpiritSpeakCalled = false;
    
    private bool isIntroCalled = false;
    private bool isOutroCalled = false;
    private bool isSpecialImageCalled = false;

    
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
        
        if (!isIntroCalled)
        {
            // 如果DisplayIntro没有被调用，就清空显示
            m_introText.text = "";
            m_introImage.sprite = null;
            m_introGroup.SetActive(false);
        }
        
        if (!isOutroCalled)
        {
            // 如果DisplayOutro没有被调用，就清空显示
            m_outroText.text = "";
            m_outroImage.sprite = null;
            m_outroGroup.SetActive(false);
        }

        if (!isSpecialImageCalled)
        {
            m_specialImage.sprite = null;
            m_specialImage.enabled = false;
        }

        
        
        
        // 重置isDisplayCalled为false
        isDisplayCalled = false;
        // 重置isSpiritSpeakCalled为false
        isSpiritSpeakCalled = false;
        // 重置isOutroCalled为false
        isOutroCalled = false;
        isSpecialImageCalled = false;
        isIntroCalled = false;
        

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
    
    public void DisplayIntro(SpiritSpeakEntry entry)
    {
        m_introGroup.SetActive(true);
        m_introText.text = entry.dialogueText;
        m_introImage.sprite = entry.SpiritImage;
        
        // 标记DisplayIntro已被调用
        isIntroCalled = true;
    }
    
    public void DisplayOutro(SpiritSpeakEntry entry)
    {
        m_outroGroup.SetActive(true);
        m_outroText.text = entry.dialogueText;
        m_outroImage.sprite = entry.SpiritImage;

        // 标记DisplayOutro已被调用
        isOutroCalled = true;
    }
    
    public void SwitchIntroDisplay()
    {
        m_introGroup.SetActive(!m_introGroup.activeSelf);//切换显示状态
    }
    
    public void DisplayIntroduction(SpiritSpeakEntry entry)
    {
        m_introGroup.SetActive(true);
        m_introText.text = entry.dialogueText;
        m_introImage.sprite = entry.SpiritImage;
        isIntroCalled = true;
    }

    public void DisplaySpecialImage(Sprite image)
    {
        m_specialImage.enabled = true;
        m_specialImage.sprite = image;
        isSpecialImageCalled = true;
    }
    
    public void SwitchOutroDisplay()
    {
        m_outroGroup.SetActive(!m_outroGroup.activeSelf); // 切换显示状态
    }
    
    public void DisplayScore(int errorNumber, int maxErrorNumber)
    {
        m_scoreDisplay.text = errorNumber + "/" + maxErrorNumber;
    }
}