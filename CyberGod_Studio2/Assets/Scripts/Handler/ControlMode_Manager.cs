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

public enum RepairingSubMode
{
    ERROR_REPAIR,
    CLOCKWORK_REPAIR,
}

public class ControlMode_Manager : MonosingletonTemp<ControlMode_Manager>
{
    
    //定义当前的控制模式
    [SerializeField] public ControlMode m_controlMode = ControlMode.DIALOGUE;
    [SerializeField] public RepairingSubMode m_repairingSubMode = RepairingSubMode.ERROR_REPAIR;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ControlModeSelfBehavior();
    }

    void ControlModeSelfBehavior()
    {
        switch (m_controlMode)
        {
            case ControlMode.NAVIGATION:
                SoundManager.Instance.EnableAudioSource(1, true);
                break;
            case ControlMode.REPAIRING:
                SoundManager.Instance.EnableAudioSource(1, true);

                //锁定鼠标
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case ControlMode.DIALOGUE:
                SoundManager.Instance.EnableAudioSource(1, false);
                Cursor.lockState = CursorLockMode.None;
                break;
            case ControlMode.NORMAL:
                break;
        }
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
