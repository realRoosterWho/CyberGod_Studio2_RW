using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 引入场景管理命名空间
using UnityEngine.UI; // 引入UI命名空间


public class main_bg_Logic : MonoBehaviour
{
    public Button yourButton; // 你的UI图像按钮
    public string targetScene; // 你想要跳转的场景名

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 定义一个函数，用于跳转到指定的场景
    public void ChangeScene()
    {
        SoundManager.Instance.PlaySFX(2);
        SceneManager.LoadScene(targetScene);
    }
    
    //退出软件
    public void ExitGame()
    {
        Application.Quit();//需要命名空间using UnityEngine;
    }
}