using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LayerChangeMaskEffect_Main : MonoBehaviour
{
    public Image image; // Image 组件的引用
    [Header("渐变效果的持续时间")]
    //增加范围
    [Range(0.0f, 0.5f)]
    public float fadeInDuration = 0.5f; // 从 0 渐变到 1 的持续时间
    [Range(0.0f, 0.5f)]
    
    public float fadeOutDuration = 0.5f; // 从 1 渐变到 0 的持续时间
    [Range(0.0f, 0.5f)]
    public float stayDuration = 0.5f; // 保持 0 的持续时间


    private void Start()
    {
        // 获取 Image 组件的引用
        image = GetComponent<Image>();
        
        // 订阅事件
        EventManager.Instance.AddEvent("LayerChanged", HandleLayerChange);
    }

    private void HandleLayerChange(GameEventArgs args)
    {
        // 检查对象是否为 null
        if (this == null)
        {
            return;
        }

        Debug.Log("HandleLayerChange called"); // 添加这行调试代码

        // 当层级改变时，启动渐变效果的协程
        StartCoroutine(FadeOutAndIn());
    }

    private IEnumerator FadeOutAndIn()
    {
        Color initialColor = image.color;

        // 先将 alpha 值从 0 渐变到 1
        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, t / fadeInDuration);
            image.color = newColor;
            yield return null;
        }

        // image.color = initialColor;

        // 保持 1
        yield return new WaitForSeconds(stayDuration);

        // 切换 Image 的 sprite
        // 这里应该是你的 sprite 切换代码
        // image.sprite = newSprite;

        // 再将 alpha 值从 1 渐变到 0
        for (float t = 0; t < fadeOutDuration; t += Time.deltaTime)
        {
            Color newColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1 - t / fadeOutDuration);
            image.color = newColor;
            yield return null;
        }

        image.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0);
    }
}