using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health_Handler : MonoBehaviour
{
    //定义一个类，用于表明当前生命值，维修值，精神值的状态名称，比如生命值为 Healthy,Indanger,Dead之类的
    public enum HealthStatus
    {
        HIGH,
        MID,
        LOW,
        ZERO
    }
    
    
    
    //定义生命值，维修值，精神值，以及这些数值的最大量=100
    [SerializeField]private float m_health = 100;
    [SerializeField]private float m_repair = 0;
    [SerializeField]private float m_spirit = 100;
    private float MAXHEALTH = 100;
    private float MAXREPAIR = 100;
    private float MAXSPIRIT = 100;
    
    //定义生命值，维修值，精神值的状态
    [SerializeField] private HealthStatus m_healthStatus;
    [SerializeField] private HealthStatus m_repairStatus;
    [SerializeField] private HealthStatus m_spiritStatus;
    
    //定义生命值，维修值，精神值的HealthBar,RepairBar,SpiritBar,他们都是SCROLLBAR类型
    [SerializeField] private Scrollbar HealthBar;
    [SerializeField] private Scrollbar RepairBar;
    [SerializeField] private Scrollbar SpiritBar;
    void Start()
    {
        //注册监听事件
        EventManager.Instance.AddEvent("HealthChange", OnHealthChange);
        EventManager.Instance.AddEvent("RepairChange", OnRepairChange);
        EventManager.Instance.AddEvent("SpiritChange", OnSpiritChange);
    }

    void Update()
    {
        UpdateStatus();
        UpdateShow();
    }
    
    //定义通用数值增加/减少函数
    public void ChangeValue(ref float value, float change, float maxValue)
    {
        value += change;
        if (value > maxValue)
        {
            value = maxValue;
        }
        if (value < 0)
        {
            value = 0;
        }
    }
    
    //定义生命值增加/减少函数
    public void ChangeHealth(float value)
    {
        // Debug.Log($"Health Changed: {value}");
        ChangeValue(ref m_health, value, MAXHEALTH);
    }
    
    //定义维修值增加/减少函数
    public void ChangeRepair(float value)
    {
        ChangeValue(ref m_repair, value, MAXREPAIR);
    }
    
    //定义精神值增加/减少函数
    public void ChangeSpirit(float value)
    {
        ChangeValue(ref m_spirit, value, MAXSPIRIT);
    }
    
    public void ShowStatus(float value, Scrollbar bar, float maxValue)
    {
        bar.size = value / maxValue;
    }
    

    //更新各个数值的状态
    public void UpdateStatus(ref float value, ref HealthStatus status)
    {
        if (value > 70)
        {
            status = HealthStatus.HIGH;
        }
        else if (value > 30)
        {
            status = HealthStatus.MID;
        }
        else if (value > 0)
        {
            status = HealthStatus.LOW;
        }
        else
        {
            status = HealthStatus.ZERO;
        }
    }
    
    
    //定义更新函数，用于更新生命值，维修值，精神值的显示
    public void UpdateShow()
    {
        ShowStatus(m_health, HealthBar, MAXHEALTH);
        ShowStatus(m_repair, RepairBar, MAXREPAIR);
        ShowStatus(m_spirit, SpiritBar, MAXSPIRIT);
    }
    
    //定义更新生命值，维修值，精神值的状态的函数
    public void UpdateStatus()
    {
        UpdateStatus(ref m_health, ref m_healthStatus);
        UpdateStatus(ref m_repair, ref m_repairStatus);
        UpdateStatus(ref m_spirit, ref m_spiritStatus);

        //Debug目前的生命值，维修值，精神值的状态
        Debug.Log($"HealthStatus: {m_healthStatus} RepairStatus: {m_repairStatus} SpiritStatus: {m_spiritStatus}");
    }
    
    //以下是HealthHandler监听事件所执行的方法
    public void OnHealthChange(GameEventArgs args)
    {
        ChangeHealth(args.FloatValue);
        // Debug.Log($"Health Changed: {args.FloatValue}");
    }
    
    public void OnRepairChange(GameEventArgs args)
    {
        ChangeRepair(args.FloatValue);
    }
    
    public void OnSpiritChange(GameEventArgs args)
    {
        ChangeSpirit(args.FloatValue);
    }
    
    
    
    
}
