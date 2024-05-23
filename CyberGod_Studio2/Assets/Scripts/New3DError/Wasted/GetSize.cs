using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GetSize : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject model; // Ä£ÐÍ
    void Start()
    {
        var size = transform.GetComponent<Renderer>().bounds.size;
        float3 center = transform.GetComponent<Renderer>().bounds.center;
        float3 center2 = model.transform.position;
        Debug.Log("center:( " + center.x+"," +center.y + "," + center.z +")");
        Debug.Log("size:( " + size.x + "," + size.y + "," + size.z + ")");
        Debug.Log("center2: ( " + center2.x + "," + center2.y + "," + center2.z + ")");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
