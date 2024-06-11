using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntro_Logic : MonoBehaviour
{
    private Image imageComponent;
    [SerializeField] private List<string> introTextList;
    [SerializeField] private List<string> outroTextList;
    
    [SerializeField] private List<string> outro_IntroRenderTextList;
    private int currentTextIndex = 0;
    private bool isClicked = false;
    private bool isIntroFinished = false;
    private bool isOutroStarted = false;
    private bool isOutroIntroFinished = false;


    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.enabled = true;
        EventManager.Instance.AddEvent("OnWinning", StartOutro);
    }

    // Update is called once per frame
    void Update()
    {
        isClicked = false;
        if (ControlMode_Manager.Instance.m_controlMode == ControlMode.NAVIGATION)
        {
            imageComponent.enabled = false;
        }
        else if (ControlMode_Manager.Instance.m_controlMode == ControlMode.DIALOGUE)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isClicked = true;
            }
            if (!isIntroFinished)
            {
                RequestNextIntroText();
            }
            else if (isOutroStarted)
            {
                RequestNextOutroText();
            }
        }
    }

    public void RequestNextIntroText()
    {
        if (isClicked)
        {
            SoundManager.Instance.PlaySFX(2);
            currentTextIndex++;
        }
        
        if (currentTextIndex < introTextList.Count)
        {
            DialogueManager.Instance.RequestIntroEntry(introTextList[currentTextIndex]);
            Debug.Log("Requesting: " + introTextList[currentTextIndex]);
        }
        if (currentTextIndex >= introTextList.Count)
        {
            isIntroFinished = true;
            currentTextIndex= 0;
            UIDisplayManager.Instance.SwitchIntroDisplay();
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
            EventManager.Instance.TriggerEvent("IntroDone", new GameEventArgs());
        }
    }
    
    public void RequestNextOutroText()
    {
        if (isClicked)
        {
            SoundManager.Instance.PlaySFX(2);
            currentTextIndex++;
        }
        
        if (!isOutroIntroFinished)
        {
            if (currentTextIndex < outro_IntroRenderTextList.Count)
            {
                DialogueManager.Instance.RequestOutroEntry(outro_IntroRenderTextList[currentTextIndex]);
                Debug.Log("Requesting: " + outro_IntroRenderTextList[currentTextIndex]);
            }
            if (currentTextIndex >= outro_IntroRenderTextList.Count)
            {
                isOutroIntroFinished = true;
                currentTextIndex= 0;
                UIDisplayManager.Instance.SwitchIntroDisplay();
                UIDisplayManager.Instance.SwitchOutroDisplay();
            }
        }
        else
        {
            if (currentTextIndex < outroTextList.Count)
            {
                DialogueManager.Instance.RequestOutroEntry(outroTextList[currentTextIndex]);
                Debug.Log("Requesting: " + outroTextList[currentTextIndex]);
            }
            if (currentTextIndex >= outroTextList.Count)
            {
                //前往下一个场景
                m_GameManager.Instance.ChangeScene("Win");
            }
        }
        
    }
    
    public void StartOutro(GameEventArgs args)
    {
        if (!isOutroStarted)
        {
            if (outro_IntroRenderTextList.Count > 0)
            {
                isOutroIntroFinished = false;
                UIDisplayManager.Instance.SwitchIntroDisplay();
            }
            else
            {
                isOutroIntroFinished = true;
                UIDisplayManager.Instance.SwitchOutroDisplay();
            }
        }

        isOutroStarted = true;
        
        ControlMode_Manager.Instance.ChangeControlMode(ControlMode.DIALOGUE);
    }
}