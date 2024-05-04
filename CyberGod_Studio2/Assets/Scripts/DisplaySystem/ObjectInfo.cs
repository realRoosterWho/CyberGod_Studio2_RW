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