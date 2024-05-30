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
    [SerializeField] private Dictionary<string, SpiritSpeakEntry> m_introOutroEntryDict; // 新增

    public List<SpiritSpeakEntry> activeSpiritSpeakEntries = new List<SpiritSpeakEntry>();
    public List<SpiritSpeakEntry> activeIntroOutroEntries = new List<SpiritSpeakEntry>(); // 新增

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
        activeIntroOutroEntries.Clear(); // 新增
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
    
    public void RequestIntroOutroEntry(string textName) // 新增
    {
        if (m_introOutroEntryDict.ContainsKey(textName))
        {
            SpiritSpeakEntry entry = m_introOutroEntryDict[textName];
            activeIntroOutroEntries.Add(entry);
            activeIntroOutroEntries = activeIntroOutroEntries.OrderBy(x => x.priority).ToList();
            UpdateIntroOutroDisplay();
        }
        else
        {
            Debug.LogError("No entry found with textName: " + textName);
        }
    }

    public void UpdateIntroOutroDisplay() // 新增
    {
        if (activeIntroOutroEntries.Count > 0)
        {
            SpiritSpeakEntry entry = activeIntroOutroEntries[0];
            DisplayIntroOutro(entry);
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

    public void DisplaySpriteSpeak(SpiritSpeakEntry entry)
    {
        UIDisplayManager.Instance.DisplaySpiritSpeak(entry);
    }
    
    public void DisplayIntroOutro(SpiritSpeakEntry entry)
    {
        UIDisplayManager.Instance.DisplayIntroOutro(entry);
    }
}