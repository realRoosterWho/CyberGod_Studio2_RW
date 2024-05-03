using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechaniclayer_Logic : MonoBehaviour
{
    
    Clockwork_Logic m_clockworkLogic;
    public bool hasClockwork = false;
    
    private float m_time = 0.0f;
    private float lasttime = 0.0f;
    private float currentTime = 0.0f;
    
    private float m_inputTimer = 0.0f;
    private float INPUT_INTERVAL = 1.0f;
    
    private bool isWindingToMax = false;
    // Start is called before the first frame update
    void Start()
    {
        //从子物体中获取Clockwork_Logic组件
        m_clockworkLogic = GetComponentInChildren<Clockwork_Logic>();
        
        EventManager.Instance.AddEvent("WindingToMax", OnWindingToMax);

    }

    // Update is called once per frame
    void Update()
    {
        //更新时间
        m_time += Time.deltaTime;
        
        //如果获取到了时钟，那么更新hasClockwork
        if(m_clockworkLogic != null)
        {
            hasClockwork = true;
        }
        else 
        {
            hasClockwork = false;
        }
    }
    
    void LateUpdate()
    {
        
    }

    public void HandleClockworkInput()//只有速度到一定程度的时候再工作
    {
        Debug.Log("HandleClockworkInput");
        // hasClockworkInput = true;
        lasttime = m_time;
        
        //Debug currentTime and lasttime
        Debug.Log("currentTime: " + currentTime);
        Debug.Log("lasttime: " + lasttime);
    }

    public void ClockworkRepairing()//只要Repairing一直工作
    {
        currentTime = m_time;

        // 在这里使用currentTime和lastTime来判断m_clockworkLogic的状态
        if(currentTime - lasttime > 1.0f)
        {
            m_clockworkLogic.m_clockworkState = ClockworkState.COUNTING;
        }
        else
        {
            m_clockworkLogic.m_clockworkState = ClockworkState.WINDING;
        }
    }
    
    private void OnWindingToMax(GameEventArgs args)
    {
        isWindingToMax = true;
    }
}
