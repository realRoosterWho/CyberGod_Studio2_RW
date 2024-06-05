using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAnimation : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] private float playSpeed = 1.0f; // 控制播放速度
    [SerializeField] [Range(0, 1)] private float transparency = 1.0f; // 控制材质的透明度
    [SerializeField] private Sprite[] spriteArray; // 存储序列帧动画

    private SpriteRenderer spriteRenderer; // SpriteRenderer组件
    private int currentFrame; // 当前帧
    private float timer; // 计时器

    // Start is called before the first frame update
    void Start()
    {
        // 获取SpriteRenderer组件
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("没有找到SpriteRenderer组件！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;
        float playFactor = playSpeed * 100;// 播放因子
        // 如果计时器大于等于1/playSpeed
        if (timer >= 1/playFactor)
        {
            // 更新当前帧
            currentFrame = (currentFrame + 1) % spriteArray.Length;
            // 更新SpriteRenderer的Sprite
            spriteRenderer.sprite = spriteArray[currentFrame];
            // 重置计时器
            timer -= 1/playFactor;
        }

        // 更新SpriteRenderer的颜色，修改透明度
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, transparency);
    }
}