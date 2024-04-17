using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Layer
{
    FLESH,
    MACHINE,
    NERVE
}

public class Layer_Handler : MonosingletonTemp<Layer_Handler>
{
    //定义一个枚举类型，用于表示当前的层级

    
    //定义当前的层级
    [SerializeField] public Layer m_layer = Layer.FLESH;
    
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
