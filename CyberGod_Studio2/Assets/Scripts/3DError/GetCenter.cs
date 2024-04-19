using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetCenter : MonoBehaviour
{
    public GameObject Neko;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Neko's position:" + Neko.transform.position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
