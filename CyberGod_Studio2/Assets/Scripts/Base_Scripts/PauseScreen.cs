using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// 附加脚本：
//
// 把 PauseScreen 脚本添加到表示暂停菜单的UI面板上。
// 配置UI：
//
// 在Unity的Inspector面板中，确保UI面板已设置为脚本的目标对象。
// 功能实现：
//
// Setup()：激活暂停菜单界面。
// SetDown()：关闭暂停菜单界面。
// QuitButton()：退出游戏。
// RestartButton()：重启当前游戏场景。
// ResumeButton()：调用 GameManager 的 ResumeGame() 方法，继续游戏。

public class PauseScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public void Setup()
    {
        gameObject.SetActive(true); //激活自己
    }
    
    public void SetDown()
    {
        gameObject.SetActive(false); //关闭自己
    }
    
    public void QuitButton()
    {
        //退出游戏
        Application.Quit();
    }


    public void RestartButton()
    {
        //加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
    public void ResumeButton()
    {
        //继续游戏
        m_GameManager.Instance.ResumeGame();
    }
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;


    }
}
