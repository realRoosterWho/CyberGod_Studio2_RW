using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction_Trigger : MonoBehaviour
{
    Layer_Handler m_layerHandler;
    
    
    private bool isNerveIntroDone = false;
    private bool isSecondFleshIntroDone = false;
    private bool isSomethingRepairedIntroDone = false;
    
    private bool isOnSomethingRepaired = false;
    
    // Start is called before the first frame update
    void Start()
    {
        m_layerHandler = Layer_Handler.Instance;
        EventManager.Instance.AddEvent("SomethingRepaired", OnSomethingRepaired);

    }

    // Update is called once per frame
    void Update()
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
    
    private void OnSomethingRepaired(GameEventArgs args)
    {
        // 这里添加你的代码
        isOnSomethingRepaired = true;
    }
}
