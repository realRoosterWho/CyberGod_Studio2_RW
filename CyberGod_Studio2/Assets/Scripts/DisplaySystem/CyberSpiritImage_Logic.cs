using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberSpiritImage_Logic : MonoBehaviour
{
    public float amplitude = 0.5f; // 振幅
    public float frequency = 1f; // 频率

    private Vector2 initialPosition;
    private RectTransform rectTransform;

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        StartCoroutine(Breathe());
    }

    void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        initialPosition = rectTransform.anchoredPosition;
        StartCoroutine(Breathe());
    }

    void OnDisable()
    {
        StopCoroutine(Breathe());
    }

    IEnumerator Breathe()
    {
        while (true)
        {
            // 确保 rectTransform 已经被初始化
            if (rectTransform == null)
            {
                yield return null;
                continue;
            }

            float y = amplitude * Mathf.Sin(Time.time * frequency);
            rectTransform.anchoredPosition = new Vector2(initialPosition.x, initialPosition.y + y);
            yield return null;
        }
    }
}