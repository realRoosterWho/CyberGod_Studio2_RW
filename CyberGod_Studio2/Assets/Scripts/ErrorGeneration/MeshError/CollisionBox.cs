using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox: MonoBehaviour
{
    
    private bool isClicked = false;

    
    //获取自己的材质
    private Material m_material;
    private void Start()
    {
        //获取自己的材质
        m_material = GetComponent<Renderer>().material;
        //让自己材质的透明度降低，我的材质用的是Sprite-Default
        Color color = m_material.color;
        color.a = 0.05f;
        m_material.color = color;

    }
    
    void FixedUpdate()
    {
        isClicked = false;
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("3DError") && isClicked)
        {
            StartCoroutine(DestroyAfterDelay(other.gameObject, 0.25f));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.Instance.TriggerEvent("ErrorDestroyed", new GameEventArgs());
        Destroy(gameObject);
        isClicked = false;
    }
}
