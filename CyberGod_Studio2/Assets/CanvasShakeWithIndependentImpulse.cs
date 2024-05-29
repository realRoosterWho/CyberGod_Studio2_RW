using UnityEngine;
using Cinemachine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CanvasShakeWithIndependentImpulse : MonoBehaviour
{
    private CinemachineIndependentImpulseListener impulseListener;
    private Canvas[] allCanvases;
    private Dictionary<RectTransform, Vector2> initialPositions = new Dictionary<RectTransform, Vector2>();
    private Dictionary<RectTransform, float> lerpProgresses = new Dictionary<RectTransform, float>();


    private void Start()
    {
        impulseListener = GetComponent<CinemachineIndependentImpulseListener>();
        if (impulseListener == null)
        {
            Debug.LogError("没有找到CinemachineIndependentImpulseListener组件！");
        }

        // 获取场景中的所有 Canvas
        allCanvases = FindObjectsOfType<Canvas>();

        // 记录所有 RectTransform 的初始位置
        foreach (var canvas in allCanvases)
        {
            foreach (RectTransform rectTransform in canvas.GetComponentsInChildren<RectTransform>())
            {
                initialPositions[rectTransform] = rectTransform.anchoredPosition;
            }
        }
    }

    private void LateUpdate()
    {
        if (impulseListener != null)
        {
            // 这里调用CinemachineImpulseManager来检测是否有Impulse
            bool haveImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(
                transform.position, impulseListener.m_Use2DDistance, impulseListener.m_ChannelMask,
                out Vector3 impulsePos, out Quaternion impulseRot);

            if (haveImpulse)
            {
                Debug.Log("检测到Impulse！");

                // 遍历所有 Canvas
                foreach (var canvas in allCanvases)
                {
                    // 遍历 Canvas 中的所有 UI 元素
                    foreach (RectTransform rectTransform in canvas.GetComponentsInChildren<RectTransform>())
                    {
                        // 对 UI 元素的 RectTransform 进行操作以使它们振动
                        rectTransform.anchoredPosition += new Vector2(impulsePos.x, impulsePos.y);

                        // 有冲击力时，重置插值进度
                        if (lerpProgresses.ContainsKey(rectTransform))
                        {
                            lerpProgresses[rectTransform] = 0f;
                        }
                        else
                        {
                            lerpProgresses.Add(rectTransform, 0f);
                        }
                    }
                }
            }
            else
            {
                // 没有冲击力时，将所有 RectTransform 的位置平滑地插值到其初始位置
                foreach (var rectTransform in initialPositions.Keys)
                {
                    if (lerpProgresses.ContainsKey(rectTransform))
                    {
                        lerpProgresses[rectTransform] += Time.deltaTime;
                        float t = lerpProgresses[rectTransform];

                        // 使用 Lerp 方法进行插值
                        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, initialPositions[rectTransform], t);
                    }
                }
            }
        }
    }
}