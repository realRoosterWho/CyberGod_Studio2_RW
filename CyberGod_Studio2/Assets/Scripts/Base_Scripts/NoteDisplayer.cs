using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入TextMeshPro命名空间

// 附加脚本：
// 将 NoteDisplayer 脚本添加到含有 TextMeshProUGUI 组件的UI文本对象上。

// 配置文本数组：
// 在Unity编辑器的Inspector面板中，将你想展示的所有文本字符串添加到 notes 数组中。

// 显示文本：
// 运行游戏后，脚本会在TextMeshPro文本组件上显示 notes 数组中的第一段文本。

// 交互显示：
// 在游戏运行时，每次点击鼠标左键，UI文本组件就会显示 notes 数组中的下一段文本。
// 当所有文本都显示完毕后，如果继续点击鼠标左键，文本显示区域将清空。


public class NoteDisplayer : MonoBehaviour
{
    public string[] notes; // 存储所有的文字
    private int currentNoteIndex = 0; // 当前显示的文字的索引
    public TextMeshProUGUI textMeshPro; // TextMeshPro文本组件

    // Start is called before the first frame update
    void Start()
    {
        //获取自己的TextMeshPro
        textMeshPro = GetComponent<TextMeshProUGUI>();
        
        if (notes.Length > 0)
        {
            textMeshPro.text = notes[currentNoteIndex]; // 在TextMeshPro文本组件中显示第一段文字
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 检查特定的条件，这里假设条件是按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            currentNoteIndex++; // 显示下一段文字

            if (currentNoteIndex < notes.Length)
            {
                textMeshPro.text = notes[currentNoteIndex]; // 在TextMeshPro文本组件中显示下一段文字
            }
            else
            {
                textMeshPro.text = ""; // 如果没有更多的文字，就在TextMeshPro文本组件中显示提示信息
            }
        }
    }
}