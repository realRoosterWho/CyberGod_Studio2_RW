using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetermineBoxUILogic : MonoBehaviour
{
    
    //获取自身的UI组件
    private RawImage m_image;
    private bool m_canShow = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //MeshErrorExists AddEvent
        EventManager.Instance.AddEvent("MeshErrorExists", OnMeshErrorExists);
        m_image = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCanShow();
        m_canShow = false;
    }
    
    void UpdateCanShow()
    {
        if (m_canShow)
        {
            m_image.enabled = true;
        }
        else
        {
            m_image.enabled = false;
        }

    }
    
    private void OnMeshErrorExists(GameEventArgs args)
    {
        
        if (Layer_Handler.Instance.m_layer == Layer.FLESH)
        {
            m_canShow = true;
        }
        else
        {
            m_canShow = false;
        }
    }
    
}
