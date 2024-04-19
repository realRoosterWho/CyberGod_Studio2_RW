using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neko : MonoBehaviour
{
    // Start is called before the first frame update
    private float a = 1f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, a, 0));


    }
}
