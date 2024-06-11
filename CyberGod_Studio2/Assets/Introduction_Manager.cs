using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Introduction_Manager : MonoBehaviour
{
    private List<string> introTextList0; 
    [SerializeField] private List<string> introTextList1; // 对应IntroDone事件
    [SerializeField] private List<string> introTextList2; // 对应GettingIntoNerveFirstTime事件
    [SerializeField] private List<string> introTextList3; // 对应GettingIntoFleshSecondTime事件
    [SerializeField] private List<string> introTextList4; // 对应GettingIntoRepairFirstTime事件
    [SerializeField] private List<string> introTextList5; // 对应SomethingRepairedFirstTime事件
    [SerializeField] private int specialDisplayIndex;
    private int currentTextIndex = 0;
    private bool isClicked = false;
    private bool isIntroFinished = true;

    void Start()
    {
        // 订阅事件
        EventManager.Instance.AddEvent("IntroDone", OnIntroDone);
        EventManager.Instance.AddEvent("GettingIntoNerveFirstTime", OnGettingIntoNerveFirstTime);
        EventManager.Instance.AddEvent("GettingIntoFleshSecondTime", OnGettingIntoFleshSecondTime);
        EventManager.Instance.AddEvent("GettingIntoRepairFirstTime", OnGettingIntoRepairFirstTime);
        EventManager.Instance.AddEvent("SomethingRepairedFirstTime", OnSomethingRepairedFirstTime);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }
        
        if (!isIntroFinished)
        {
            RequestNextIntroText();
        }
    }

    public void RequestNextIntroText()
    {
        if (isClicked)
        {
            SoundManager.Instance.PlaySFX(2);
            currentTextIndex++;
            isClicked = false;
        }

        if (currentTextIndex < introTextList1.Count)
        {
            if (currentTextIndex >= introTextList0.Count - specialDisplayIndex)
            {
                // 关闭IntroDisplay的显示
                UIDisplayManager.Instance.SwitchIntroDisplay();
                
                // 只显示SpecialImage
                UIDisplayManager.Instance.DisplaySpecialImage(DialogueManager.Instance.GetImageByEntry(introTextList0[currentTextIndex]));
            }
            else
            {
                // 显示IntroEntry
                DialogueManager.Instance.RequestIntroductionEntry(introTextList0[currentTextIndex]);
                Debug.Log("Requesting: " + introTextList0[currentTextIndex]);
            }
        }
        if (currentTextIndex >= introTextList0.Count)
        {
            isIntroFinished = true;
            currentTextIndex = 0;
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
        }
    }

    public void StartIntroduction(List<string> introTexts, int specialIndex)
    {
        introTextList0 = introTexts;
        specialDisplayIndex = specialIndex;
        isIntroFinished = false;
        currentTextIndex = 0;
        ControlMode_Manager.Instance.ChangeControlMode(ControlMode.DIALOGUE);
        UIDisplayManager.Instance.SwitchIntroDisplay();
    }

    private void OnIntroDone(GameEventArgs args)
    {
        Debug.Log("IntroDone");
        StartIntroduction(introTextList1, specialDisplayIndex);
    }

    private void OnGettingIntoNerveFirstTime(GameEventArgs args)
    {
        isClicked = false;
        StartIntroduction(introTextList2, specialDisplayIndex);
    }

    private void OnGettingIntoFleshSecondTime(GameEventArgs args)
    {
        isClicked = false;
        StartIntroduction(introTextList3, specialDisplayIndex);
    }

    private void OnGettingIntoRepairFirstTime(GameEventArgs args)
    {
        isClicked = false;
        StartIntroduction(introTextList4, specialDisplayIndex);
    }

    private void OnSomethingRepairedFirstTime(GameEventArgs args)
    {
        isClicked = false;
        StartIntroduction(introTextList5, specialDisplayIndex);
    }
}