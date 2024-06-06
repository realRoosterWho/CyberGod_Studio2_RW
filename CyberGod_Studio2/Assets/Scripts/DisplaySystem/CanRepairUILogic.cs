using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanRepairUILogic : MonoBehaviour
{
    [SerializeField]
    private float blinkPeriod = 1f; // 闪烁周期
    [SerializeField]
    private float minAlpha = 0.5f; // 最低的Alpha值

    private Image m_image;
    private TextMeshProUGUI m_text;
    private bool m_canShow = false;
    private Color m_color;

    void Start()
    {
        m_image = GetComponent<Image>();
        m_text = GetComponentInChildren<TextMeshProUGUI>();
        EventManager.Instance.AddEvent("CanRepairSomething", OnCanRepairSomething);
        StartCoroutine(Blink());
    }

    void Update()
    {
        m_image.enabled = m_canShow;
        m_text.enabled = m_canShow;
        m_canShow = false;
    }

    IEnumerator Blink()
    {
        while (true)
        {
            float alpha = (Mathf.Sin(Time.time / blinkPeriod) + 1) / 2 * (1 - minAlpha) + minAlpha;
            m_color = m_image.color;
            m_color.a = alpha;
            m_image.color = m_color;
            yield return null;
        }
    }

    private void OnCanRepairSomething(GameEventArgs args)
    {
        m_canShow = true;
    }
}
