using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
using UnityEditor.PackageManager;

#endif
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Add3DErrorByCube : SerializedMonoBehaviour
{
    public GameObject scaleCube;
    public GameObject model;
    public RaycastHit hit = new RaycastHit();
    public Ray ray;
    public GameObject prefab3DError;
    static public bool ifAddDone = false;


    // ������з�Χcube���б�
    // ͬһ�������Ӧ�ķ�ΧcubeӦ������
    public List<GameObject> scaleCubes;
    // ����������Ϊ����ָʾ�洢�����巶Χcube��������Χ��
    [ShowInInspector]
    public Dictionary<string, List<int>> scaleCubesIndex = new Dictionary<string, List<int>>();
    [ShowInInspector]
    public Dictionary<string, GameObject> models = new Dictionary<string, GameObject>();


    // ȡGameObject��ǰ�˸����㣨��������Cube)
    private List<Vector3> GetWorldPositionOfVertexs(GameObject scaleCube)
    {
        return scaleCube.GetComponent<MeshFilter>()
            .sharedMesh
            .vertices
            .Select(v => scaleCube.transform.TransformPoint(v))
            .Take(8).ToList();
    }

    // �����cube����ȡһ��
    private Vector3 GetRandomVectorAtCube(List<Vector3> vec)
    {
        Vector3 res = new Vector3();
        // ���һ����
        int randomStableAxis = Random.Range(0, 3);
        int randomAxisValue = Random.Range(0, 2);

        switch(randomStableAxis)
        {
            case 0:
                // x��̶�
                res.x = randomAxisValue == 0 ? vec[1].x : vec[0].x;
                res.y = Random.Range(Mathf.Min(vec[1].y, vec[3].y),
                    Mathf.Max(vec[1].y, vec[3].y));
                res.z = Random.Range(Mathf.Min(vec[3].z, vec[5].z),
                    Mathf.Max(vec[3].z, vec[5].z));
                break;
            case 1:
                // y��̶�
                res.x = Random.Range(Mathf.Min(vec[1].x, vec[0].x),
                    Mathf.Max(vec[1].x, vec[0].x));
                res.x = randomAxisValue == 0 ? vec[0].y : vec[2].y;
                res.z = Random.Range(Mathf.Min(vec[0].z, vec[6].z),
                    Mathf.Max(vec[0].z, vec[6].z));
                break;
            case 2:
                // z��̶�
                res.x = Random.Range(Mathf.Min(vec[1].x, vec[0].x),
                    Mathf.Max(vec[1].x, vec[0].x));
                res.y = Random.Range(Mathf.Min(vec[1].y, vec[3].y),
                    Mathf.Max(vec[1].y, vec[3].y));
                res.z = randomAxisValue == 0 ? vec[0].z : vec[4].z;
                break;
        }
        return res;
    }

    private void StartRay(GameObject scaleCube)
    {
        List<Vector3> vec = GetWorldPositionOfVertexs(scaleCube);
        Vector3 origin = GetRandomVectorAtCube(vec);
        Vector3 end = scaleCube.transform.position;

        // ��������������
        ray = new Ray();
        ray.origin = origin;
        ray.direction = end - origin;
        // Debug.DrawRay(ray.origin, (end - origin), Color.red);
        // Debug.Log(myGameObject.transform.position);
    }


    public void hitWhileOn(GameObject scaleCube)
    {
        StartRay(scaleCube);
        while (!Physics.Raycast(ray, out hit, 100))
        {
            StartRay(scaleCube);
        }
    }

    public void hitWhileOnModel(GameObject scaleCube)
    {
        hitWhileOn(scaleCube);
        // �Ѿ�����
        while (hit.collider.CompareTag("3DError") || hit.collider.CompareTag("Box") || hit.collider.CompareTag("ScaleCube"))
        {
            hitWhileOn(scaleCube);
        }
    }
    public void GenerateMeshErrorAtArea(GameObject scaleCube, GameObject smodel)
    {
        hitWhileOnModel(scaleCube);
        Debug.Log(hit.collider.gameObject.name);


        // ���ɴ�����Object
        GameObject tderror = Instantiate(prefab3DError, transform);
        tderror.transform.parent = smodel.transform;
        tderror.transform.localScale /= 3; // set the scale of error

        // ��ȡ��ײλ����Ϣ
        tderror.transform.position = hit.point;
        tderror.transform.up = hit.normal;

        // ���ô�����Object
        tderror.transform.Translate(Vector3.up * 0.15f * tderror.transform.localScale.y, Space.Self);

      
    }



    void Update()
    {
        // ����Ҽ�����һ�������������������
        /*
        if (Input.GetMouseButtonDown(1)&& !ifAddDone)
        {
            GenerateMeshErrorAtArea(scaleCube, model);
            ifAddDone = true;
        }
        */
        // ������Ҫ������1ÿ�����ָ�����һ������


        

        if (Input.GetMouseButtonDown(1) && !ifAddDone)
        {
            List<int> indexRange = new List<int>();
            GameObject smodel = new GameObject();
            scaleCubesIndex.TryGetValue("1", out indexRange);
            models.TryGetValue("1", out smodel);
            GameObject scale = new GameObject();
            for (int i = indexRange[0]; i <= indexRange[1];i++)
            {
                scale = scaleCubes[i];
                GenerateMeshErrorAtArea(scale,smodel);
            }
            ifAddDone = true;
        }



    }

}
