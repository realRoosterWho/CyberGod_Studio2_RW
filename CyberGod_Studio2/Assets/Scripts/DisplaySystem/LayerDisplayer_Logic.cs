using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LayerDisplayer_Logic : MonoBehaviour
{
    //存储TextMeshProUGUI组件
    TextMeshProUGUI m_textMeshProUGUI; // Correct class name is TextMeshProUGUI
    
    //存储图片组件
    Image m_image;
    
    //序列化存储一个Layer
    [SerializeField] public Layer m_layer;
    
    //存储几个图片
    public Sprite[] m_sprites;
    
    // Start is called before the first frame update
    void Start()
    {
        
        //获取Image组件
        m_image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //如果当前Layer是m_layer
        if (Layer_Handler.Instance.m_layer == m_layer)
        {
            //如果m_image不为空
            if (m_image != null)
            {
                //设置m_image的sprite为m_sprites[1]
                m_image.sprite = m_sprites[1];
            }
        }
        else
        {
            //如果m_image不为空
            if (m_image != null)
            {
                //设置m_image的sprite为m_sprites[0]
                m_image.sprite = m_sprites[0];
            }
        }
        
        
        
    }
}