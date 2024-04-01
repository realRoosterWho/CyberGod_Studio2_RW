using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMode_Manager : MonoBehaviour
{
    
    //定义一个枚举类型，用于表示当前的控制模式
    public enum ControlMode
    {
        NAVIGATION,
        REPAIRING,
        DIALOGUE
    }
    
    //定义当前的控制模式
    [SerializeField] private ControlMode m_controlMode = ControlMode.NAVIGATION;
    
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
    }
}
