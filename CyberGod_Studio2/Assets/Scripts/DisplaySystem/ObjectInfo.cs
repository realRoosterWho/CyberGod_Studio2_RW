using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ObjectInfo {
    [LabelText("Object Name")]  // 更改显示的标签名称
    [LabelWidth(150)]           // 设置标签的宽度
    public string name;

    [TextArea(5, 10)]            // 将描述字段转换为文本区域，设置最小行和最大行
    [LabelWidth(150)]           // 设置标签的宽度
    [PropertyOrder(0)]          // 设置属性的顺序
    [LabelText("Description")]
    public string description;

    [LabelWidth(150)]           // 设置标签的宽度
    [PropertyOrder(0)]
    public Sprite image;
}

[System.Serializable]
public class SpiritSpeakEntry {
    
    // [LabelText("TextName")]
    // public string textName;
    
    [LabelText("Spirit Image")]
    public Sprite SpiritImage;

    [TextArea(3, 5)]
    [LabelText("Dialogue Text")]
    public string dialogueText;
    
    
    
    [LabelText("Priority")]
    public int priority;  // 新增属性：对话的优先级
    
}