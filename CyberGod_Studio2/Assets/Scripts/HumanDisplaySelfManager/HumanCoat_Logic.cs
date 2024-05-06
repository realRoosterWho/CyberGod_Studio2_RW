using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClothDisplayType
{
    Coat,
    Cloth,
    Shadow,
    Nerve,
}

public class HumanCoat_Logic : MonoBehaviour
{
    
    public bool isCoatAlpha = false;
    
    //获取Layer
    [SerializeField] private ClothDisplayType m_clothDisplayType;
    //获取SpriteRenderer
    private SpriteRenderer m_spriteRenderer;
    
    //获取Layer
    private Layer m_layer;
    // Start is called before the first frame update
    void Start()
    {
        
        //获取我自己的SpriteRenderer
        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        m_layer = Layer_Handler.Instance.m_layer;

        
        //针对不同的ClothDisplayType，执行不同的操作
        switch (m_clothDisplayType)
        {
            case ClothDisplayType.Coat:
                CoatDisplay();
                break;
            case ClothDisplayType.Cloth:
                ClothDisplay();
                break;
            case ClothDisplayType.Shadow:
                ShadowDisplay();
                break;
            case ClothDisplayType.Nerve:
                NerveDisplay();
                break;
        }
    }
    
    //定义一个函数，用于显示Coat
    public void CoatDisplay()
    {
        //如果当前的Layer是Flesh，就显示Coat
        if (m_layer == Layer.FLESH)
        {
            m_spriteRenderer.enabled = true;

            //显示Coat
            if (isCoatAlpha)
            {
                m_spriteRenderer.color = new Color(1, 1, 1, 0.2f);
            }
            else
            {
                m_spriteRenderer.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            //隐藏Coat
            m_spriteRenderer.enabled = false;
        }
    }
    
    //定义一个函数，用于显示Cloth
    public void ClothDisplay()
    {
        //如果当前的Layer是FLESH，就显示Cloth
        if (m_layer == Layer.FLESH)
        {
            //显示Cloth
            m_spriteRenderer.enabled = true;
        }
        else
        {
            //隐藏Cloth
            m_spriteRenderer.enabled = false;
        }
    }
    
    //定义一个函数，用于显示Shadow
    public void ShadowDisplay()
    {
        //如果当前的Layer是FLESH，就显示Shadow
        if (m_layer == Layer.MACHINE)
        {
            //显示Shadow
            m_spriteRenderer.enabled = true;
        }
        else
        {
            //隐藏Shadow
            m_spriteRenderer.enabled = false;
        }
    }
    
    //定义一个函数，用于显示Nerve
    public void NerveDisplay()
    {
        //如果当前的Layer是Nerve，就显示Nerve
        if (m_layer == Layer.NERVE)
        {
            //显示Nerve
            m_spriteRenderer.enabled = true;
        }
        else
        {
            //隐藏Nerve
            m_spriteRenderer.enabled = false;
        }
    }
}
