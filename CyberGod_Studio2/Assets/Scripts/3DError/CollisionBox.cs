using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionBox: MonoBehaviour
{
    private void Start()
    {

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
