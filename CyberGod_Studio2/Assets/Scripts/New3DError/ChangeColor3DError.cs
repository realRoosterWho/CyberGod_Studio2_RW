using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor3DError: MonoBehaviour
{

    public Material[] material;
    Renderer rend;


    private void ChangeColorInactive()
    {
        rend.sharedMaterial = material[0];
    }
    private void ChangeColorActive()
    {
        rend.sharedMaterial = material[1];
    }

    private void RepairError()
    {
        rend.sharedMaterial = material[1];
    }

    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    void OnTriggerExit(Collider other)
    {
        // Debug.Log("exittrigger");

        if (other.gameObject.CompareTag("Box"))
        {
            ChangeColorInactive();
        }
    }
    void OnTriggerStay(Collider other)
    {
        // Debug.Log("stay");

        if (other.gameObject.CompareTag("Box"))
        {   
            // 处在判定区域内将error颜色调整至可以被消除的颜色
            ChangeColorActive();
        }
    }
  
}
