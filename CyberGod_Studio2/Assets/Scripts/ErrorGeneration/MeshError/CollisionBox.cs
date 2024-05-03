using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox: MonoBehaviour
{
    
    //获取自己的材质
    private Material m_material;
    private void Start()
    {
        //获取自己的材质
        m_material = GetComponent<Renderer>().material;
        //让自己材质的透明度降低，我的材质用的是Sprite-Default
        Color color = m_material.color;
        color.a = 0.3f;
        m_material.color = color;

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("3DError"))
        {
            if (Input.GetMouseButtonDown(0))
            {
                // need time delay
                Destroy(other.gameObject);
            }
        }
    }
}
