using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntro_Logic : MonoBehaviour
{
    private Image imageComponent;
    [SerializeField] private List<string> textList;
    private int currentTextIndex = 0;
    private bool isClicked = false;


    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.enabled = true;
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
            RequestNextText();
        }
    }

    public void RequestNextText()
    {
        if (isClicked)
        {
            SoundManager.Instance.PlaySFX(2);
            currentTextIndex++;
        }
        
        if (currentTextIndex < textList.Count)
        {
            DialogueManager.Instance.RequestIntroOutroEntry(textList[currentTextIndex]);
            Debug.Log("Requesting: " + textList[currentTextIndex]);
        }
        if (currentTextIndex >= textList.Count)
        {
            Destroy(gameObject);
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
        }
    }
}