using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;

public class DialogueManager : SerializedMonoBehaviour
{
    // 创建静态实例
    public static DialogueManager Instance { get; private set; }

    [SerializeField] private Dictionary<string, SpiritSpeakEntry> m_spriteSpeakerEntryDict;
    [SerializeField] private Dictionary<string, SpiritSpeakEntry> m_introEntryDict; // 新增
    [SerializeField] private Dictionary<string, SpiritSpeakEntry> m_outroEntryDict; // 新增
    [SerializeField] private Dictionary<string, SpiritSpeakEntry> m_introductionEntryDict; // 新增


    public List<SpiritSpeakEntry> activeSpiritSpeakEntries = new List<SpiritSpeakEntry>();
    public List<SpiritSpeakEntry> activeIntroEntries = new List<SpiritSpeakEntry>(); // 新增
    public List<SpiritSpeakEntry> activeOutroEntries = new List<SpiritSpeakEntry>(); // 新增
    public List<SpiritSpeakEntry> activeIntroductionEntries = new List<SpiritSpeakEntry>(); // 新增



    void Awake()
    {
        // 在Awake方法中初始化单例实例
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        activeSpiritSpeakEntries.Clear();
        activeIntroEntries.Clear(); // 新增
        activeOutroEntries.Clear(); // 新增
        activeIntroductionEntries.Clear(); // 新增

    }
    
    public void RequestIntroductionEntry(string textName) // 新增
    {
        if (m_introductionEntryDict.ContainsKey(textName))
        {
            SpiritSpeakEntry entry = m_introductionEntryDict[textName];
            activeIntroductionEntries.Add(entry);
            activeIntroductionEntries = activeIntroductionEntries.OrderBy(x => x.priority).ToList();
            UpdateIntroductionDisplay();
        }
        else
        {
            Debug.LogError("No entry found with textName: " + textName);
        }
    }

    public Sprite GetImageByEntry(string textName) // 新增
    {
        if (m_introductionEntryDict.ContainsKey(textName))
        {
            return m_introductionEntryDict[textName].SpiritImage;
        }
        return null;
    }

    public void RequestSpiritSpeakEntry(string textName)
    {
        if (m_spriteSpeakerEntryDict.ContainsKey(textName))
        {
            SpiritSpeakEntry entry = m_spriteSpeakerEntryDict[textName];
            activeSpiritSpeakEntries.Add(entry);
            activeSpiritSpeakEntries = activeSpiritSpeakEntries.OrderBy(x => x.priority).ToList();
            UpdateSpriteSpeakDisplay();
        }
        else
        {
            Debug.LogError("No entry found with textName: " + textName);
        }
    }
    
    public void RequestIntroEntry(string textName) // 新增
    {
        if (m_introEntryDict.ContainsKey(textName))
        {
            SpiritSpeakEntry entry = m_introEntryDict[textName];
            activeIntroEntries.Add(entry);
            activeIntroEntries = activeIntroEntries.OrderBy(x => x.priority).ToList();
            UpdateIntroDisplay();
        }
        else
        {
            Debug.LogError("No entry found with textName: " + textName);
        }
    }

    public void RequestOutroEntry(string textName) // 新增
    {
        if (m_outroEntryDict.ContainsKey(textName))
        {
            SpiritSpeakEntry entry = m_outroEntryDict[textName];
            activeOutroEntries.Add(entry);
            activeOutroEntries = activeOutroEntries.OrderBy(x => x.priority).ToList();
            UpdateOutroDisplay();
        }
        else
        {
            Debug.LogError("No entry found with textName: " + textName);
        }
    }

    public void UpdateIntroDisplay() // 新增
    {
        if (activeIntroEntries.Count > 0)
        {
            SpiritSpeakEntry entry = activeIntroEntries[0];
            DisplayIntro(entry);
        }
    }

    public void UpdateOutroDisplay() // 新增
    {
        if (activeOutroEntries.Count > 0)
        {
            SpiritSpeakEntry entry = activeOutroEntries[0];
            DisplayOutro(entry);
        }
    }
    

    public void UpdateSpriteSpeakDisplay()
    {
        if (activeSpiritSpeakEntries.Count > 0)
        {
            SpiritSpeakEntry entry = activeSpiritSpeakEntries[0];
            DisplaySpriteSpeak(entry);
        }
    }
    
    public void UpdateIntroductionDisplay() // 新增
    {
        if (activeIntroductionEntries.Count > 0)
        {
            SpiritSpeakEntry entry = activeIntroductionEntries[0];
            DisplayIntroduction(entry);
        }
    }

    public void DisplaySpriteSpeak(SpiritSpeakEntry entry)
    {
        UIDisplayManager.Instance.DisplaySpiritSpeak(entry);
    }
    
    public void DisplayIntro(SpiritSpeakEntry entry) // 新增
    {
        UIDisplayManager.Instance.DisplayIntro(entry);
    }

    public void DisplayOutro(SpiritSpeakEntry entry) // 新增
    {
        UIDisplayManager.Instance.DisplayOutro(entry);
    }
    
    public void DisplayIntroduction(SpiritSpeakEntry entry) // 新增
    {
        UIDisplayManager.Instance.DisplayIntroduction(entry);
    }
}