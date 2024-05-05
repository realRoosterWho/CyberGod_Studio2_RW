using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorLogic : MonoBehaviour
{
    
    //定义时间
    private float m_time = 0.0f;
    //定义间隔时间
    [SerializeField] private float INTERVAL = 1.0f;
    //定义对生命的伤害量
    [SerializeField] private float DAMAGE = 3.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //如果时间大于间隔时间，就TriggerHealthChangeEvent
        m_time += Time.deltaTime;
        if (m_time > INTERVAL)
        {
            TriggerHealthChangeEvent();
            m_time = 0.0f;
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

	//他自杀了qwq
	public void DestroyError()
    {
        // Call the ChangeRepair function from Health_Handler script with an input parameter of 10
        Health_Handler.Instance.ChangeRepair(10.0f);
        Destroy(gameObject);
    }
}
