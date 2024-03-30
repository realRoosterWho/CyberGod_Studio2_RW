using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// 附加脚本：
//
// 将 GameOverScreen 脚本添加到表示游戏结束界面的UI面板上。
// 配置UI组件：
//
// 在Unity的Inspector面板中，将显示分数的 TextMeshProUGUI 组件拖拽到 pointsText 字段。
// 功能实现：
//
// Setup(int points)：激活游戏结束界面并显示玩家的分数。调用这个方法时，需要传入一个整数来表示玩家得到的分数。
// RestartButton()：重启当前游戏场景，通常绑定到一个按钮上，玩家点击后可以重新开始游戏。
public class GameOverScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI pointsText;

    public void Setup(int points)
    {
        gameObject.SetActive(true);
        pointsText.text = points.ToString() + " points";
        Debug.Log("Points" + pointsText.text);
    }


    public void RestartButton()
    {
        //加载当前场景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
        
        
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
