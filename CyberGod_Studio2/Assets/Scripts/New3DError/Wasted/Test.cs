using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject myGameObject;
    public List<List<float>> haha = new List<List<float>>();

    public List<Vector3> GetWorldPositionOfVertexs(GameObject myGameObject)
    {
        return myGameObject.GetComponent<MeshFilter>()
            .sharedMesh
            .vertices
            .Select(v => myGameObject.transform.TransformPoint(v))
            .Take(8).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        var res = GetWorldPositionOfVertexs(myGameObject);
        // var list = res.Take(8).ToList<Vector3>();
        Debug.Log($"物体的顶点数量：{res.Count}");
        res.ForEach(x => Debug.Log(x));
        Debug.Log(myGameObject.transform.position);
        /*
        List<float> a = new List<float>();
        List<float> b = new List<float>();
        for (int i =0; i<=4; i++)
        {
            a.Add(0.2f);
            b.Add(0.3f);
        }
        haha.Add(a);
        haha.Add(b);
        Debug.Log(haha[0][0]);
        Debug.Log(haha[1][0]);
        */
    }
}
