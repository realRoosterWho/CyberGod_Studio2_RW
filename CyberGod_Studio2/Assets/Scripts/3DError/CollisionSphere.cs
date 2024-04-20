using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSphere : MonoBehaviour
{
    public Material[] material;
    Renderer rend;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            rend.sharedMaterial = material[1];
            if (Input.GetMouseButtonDown(0))
            {
                // add animate before destroyed
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            rend.sharedMaterial = material[0];
        }
    }
}
