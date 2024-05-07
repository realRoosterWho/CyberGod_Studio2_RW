using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 使用UnityEngine.UI，而不是Unity.UI
using TMPro;


//定义一个枚举类型，用于表示当前的时钟状态
public enum ClockworkState
{
    DEAD,
    WINDING,
    COUNTING
}
public class Clockwork_Logic : MonoBehaviour
{
    //定义当前的时钟状态
    [SerializeField] public ClockworkState m_clockworkState = ClockworkState.COUNTING;
    
    //获取TextMeshPro - Text组件
    [SerializeField] private TextMeshPro m_clockworkText;
    
    [SerializeField] private float MAXTIME = 30;
    [SerializeField] private float WINDINGSPEED = 8;
    [SerializeField] private float DAMAGE = 3;
    [SerializeField] private float INTERVAL = 1.0f;
    private float m_time = 0.0f;
    private float m_damagetimer = 0.0f;
    
    [SerializeField] private GameObject m_textMeshObject;
    
    // Start is called before the first frame update
    void Start()
    {
        m_time = MAXTIME;
        m_clockworkState = ClockworkState.COUNTING;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShow();
        UpdateStatus();
        
        //如果时间小于10，那么打开TextMeshPro - Text组件,否则关闭
        if (m_time < 10)
        {
            m_textMeshObject.SetActive(true);
        }
        else
        {
            m_textMeshObject.SetActive(false);
        }
    }
    
    void UpdateShow()
    {
        //保留一位小数
        var timetext = m_time.ToString("F1");
        m_clockworkText.text = timetext.ToString();
    }
    
    void UpdateStatus()
    {
        switch (m_clockworkState)
        {
            case ClockworkState.DEAD:
                Dead();
                break;
            case ClockworkState.WINDING:
                Winding();
                break;
            case ClockworkState.COUNTING:
                Countdown();
                break;
        }
    }
    
    void Countdown()
    {
        m_time -= Time.deltaTime;
        if (m_time <= 0)
        {
            m_time = 0;
            m_clockworkState = ClockworkState.DEAD;
        }
    }
    
    void Winding()
    {
        m_time += WINDINGSPEED * Time.deltaTime;
        if (m_time >= MAXTIME)
        {
            EventManager.Instance.TriggerEvent("WindingToMax", new GameEventArgs());
            m_time = MAXTIME;
            m_clockworkState = ClockworkState.COUNTING;
        }
    }
    
    void Dead()
    {
        m_time = 0;
        //按照间隔时间触发伤害
        m_damagetimer += Time.deltaTime;
        if (m_damagetimer > INTERVAL)
        {
            TriggerHealthChangeEvent();
            m_damagetimer = 0.0f;
        }
    }
    
    //定义一个函数，用于对生命造成伤害，通过事件的方式
    public void TriggerHealthChangeEvent()
    {
        GameEventArgs args = new GameEventArgs
        {
            FloatValue = -DAMAGE, // 设置浮点值
        };
        EventManager.Instance.TriggerEvent("HealthChange", args);
        // Debug.Log($"HealthChange: {args.FloatValue}");
    }
}
