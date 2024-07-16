using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class main_bg_Logic : MonoBehaviour
{
    public Button yourButton; // 你的UI图像按钮
    public string targetScene; // 你想要跳转的场景名
    public bool canJump = true; // 是否可以跳转

    // Start is called before the first frame update
    void Start()
    {
        canJump = false;
    }
    
    // Awake is called when the script instance is being loaded
    void Awake()
    {
        ControlMode_Manager.Instance.m_controlMode = ControlMode.DIALOGUE;

    }

    // Update is called once per frame
    void Update()
    {
        //获取场景名字，如果场景名字是Win，如果鼠标输入是Fire1,那么调用ExitGame()函数
        if (SceneManager.GetActiveScene().name == "Win" && Input.GetButtonDown("Fire1"))
        {
            ExitGame();
        }
    }

    // 定义一个函数，用于跳转到指定的场景
    public void ChangeScene()
    {
        if (!canJump)
        {
            return;
        }
        
        SoundManager.Instance.PlaySFX(2);
        SceneManager.LoadScene(targetScene);
    }
    
    //退出软件
    public void ExitGame()
    {
        Debug.Log("退出游戏");
        Application.Quit();//需要命名空间using UnityEngine;
    }
}