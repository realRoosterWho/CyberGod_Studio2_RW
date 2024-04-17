using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定义一个枚举类型，用于表示当前的控制模式
public enum ControlMode
{
    NAVIGATION,
    REPAIRING,
    DIALOGUE,
    NORMAL
}

public enum RepairingMode
{
    ERROR_REPAIR,
    CLOCKWORK_REPAIR,
}
public class ControlMode_Manager : MonosingletonTemp<ControlMode_Manager>
{
    [SerializeField] Input_Handler m_inputHandler;
    
    //定义当前的控制模式
    [SerializeField] public ControlMode m_controlMode = ControlMode.NAVIGATION;
    [SerializeField] public RepairingMode m_repairingMode = RepairingMode.ERROR_REPAIR;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //定义一个函数，用于指定地改变当前的控制模式
    public void ChangeControlMode(ControlMode controlMode)
    {
        m_controlMode = controlMode;
        
        //根据对应的控制模式，发布相应的事件//     EventManager.Instance.TriggerEvent("OnMove", args);
        switch (m_controlMode)
        {
            case ControlMode.NAVIGATION:
                EventManager.Instance.TriggerEvent("NavigationMode", new GameEventArgs());
                break;
            case ControlMode.REPAIRING:
                EventManager.Instance.TriggerEvent("RepairingMode", new GameEventArgs());
                break;
            case ControlMode.DIALOGUE:
                EventManager.Instance.TriggerEvent("DialogueMode", new GameEventArgs());
                break;
            case ControlMode.NORMAL:
                EventManager.Instance.TriggerEvent("NormalMode", new GameEventArgs());
                break;
        }
        
        
    }
}
