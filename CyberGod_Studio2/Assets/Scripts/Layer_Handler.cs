using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer_Handler : MonoBehaviour
{
    //定义一个枚举类型，用于表示当前的层级
    public enum Layer
    {
        FRESH,
        MACHINE,
        SOUL
    }
    
    //定义当前的层级
    [SerializeField] private Layer m_layer = Layer.FRESH;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //定义一个函数，用于指定地改变当前的层级
    public void ChangeLayer(Layer layer)
    {
        m_layer = layer;
    }
    //定义一个函数，用于按顺序切换当前的层级
    public void SwitchLayer()
    {
        m_layer = (Layer)(((int)m_layer + 1) % 3); //这里的3是Layer枚举类型的数量
    }

}
