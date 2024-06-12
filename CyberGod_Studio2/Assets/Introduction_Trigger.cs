using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction_Trigger : MonoBehaviour
{
    Layer_Handler m_layerHandler;
    
    [SerializeField] bool isFirstLevel = true;
    private bool isNerveIntroDone = false;
    private bool isSecondFleshIntroDone = false;
    private bool isSomethingRepairedIntroDone = false;
    
    private bool isOnSomethingRepaired = false;
    private bool isGettingIntoNerveRotation = false;
    private bool isGettingIntoMechanicFirstTime = false;
    
    // Start is called before the first frame update
    void Start()
    {
        m_layerHandler = Layer_Handler.Instance;
        EventManager.Instance.AddEvent("SomethingRepaired", OnSomethingRepaired);

    }

    // Update is called once per frame
    void Update()
    {
        if (isFirstLevel)
        {
            if (m_layerHandler.m_layer == Layer.NERVE && !isNerveIntroDone)
            {
            
                EventManager.Instance.TriggerEvent("GettingIntoNerveFirstTime", new GameEventArgs());
                // 这里添加你的代码
                isNerveIntroDone = true;
            }
            else if (m_layerHandler.m_layer == Layer.FLESH && isNerveIntroDone && !isSecondFleshIntroDone)
            {
                EventManager.Instance.TriggerEvent("GettingIntoFleshSecondTime", new GameEventArgs());
                // 这里添加你的代码
                isSecondFleshIntroDone = true;
            }
            else if (isOnSomethingRepaired && !isSomethingRepairedIntroDone)
            {
                EventManager.Instance.TriggerEvent("SomethingRepairedFirstTime", new GameEventArgs());
                // 这里添加你的代码
                isSomethingRepairedIntroDone = true;
            }
        }
        else
        {
            // 如果不是第一层
            if (m_layerHandler.m_layer == Layer.MACHINE && !isGettingIntoMechanicFirstTime)
            {
                // 如果当前层级是 MACHINE，触发 GettingIntoMechanicFirstTime 事件
                EventManager.Instance.TriggerEvent("GettingIntoMechanicFirstTime", new GameEventArgs());
                isGettingIntoMechanicFirstTime = true;
            }
            else if (m_layerHandler.m_layer == Layer.NERVE && !isGettingIntoNerveRotation)
            {
                // 如果当前层级是 NERVE，触发 GettingIntoNerveRotation 事件
                EventManager.Instance.TriggerEvent("GettingIntoNerveRotation", new GameEventArgs());
                isGettingIntoNerveRotation = true;
            }
        }
    }
    
    private void OnSomethingRepaired(GameEventArgs args)
    {
        // 这里添加你的代码
        isOnSomethingRepaired = true;
    }
}
