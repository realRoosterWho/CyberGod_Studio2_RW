#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.PackageManager;

#endif
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; // ʹ��UnityEngine.Rendering.PostProcessing��������UnityEditor.Rendering.PostProcessing
using System.Net.NetworkInformation;
// using UnityEditor.PackageManager;
using System.Collections.Generic; // Add this to use List
using Unity.Mathematics;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using System.ComponentModel;
using System.Threading;


public class Add3DError : MonoBehaviour
{

    Ray ray; // random ray to hit neko generating the position of error
    public GameObject prefab3DError;// prefab of error

    public GameObject model; // ģ��
    public RaycastHit hit;
    public float3 origin = new float3();
    public float3 endding = new float3();

    // ��������䡿�۲����߷���
    private bool rayDirection = true;

    // �����ж�ײ�����Ƿ�̫������ת��Ĵ洢ײ�����һ��������ı���
    [SerializeField] private float3 normalHitPoint = new float3();
    // �������趨�������ĵ�����ޣ����ɵĴ���С�����ֵͳͳ��Ҫ����̫����������
    [SerializeField] private float maxDistance = 0.85f;
    // �������趨����ת������ޣ����ɵĴ���С�����ֵͳͳ��Ҫ����̫����������
    [SerializeField] private float maxDistanceX = 0.2f;
    [SerializeField] private float maxDistanceZ = 0.2f;
    // �������趨����ת������ޣ����ɵĴ���С�����ֵͳͳ��Ҫ����̫����������
    [SerializeField] private float maxDistancePiot = 0.4f;
    private bool nearCenter = true;
    [SerializeField] private List<GameObject> errors = new List<GameObject>();

    static public bool ifAddDone = false;


    // 0.��������
    // 0.1��ȡ�������������xyzֵ������
    private float2 GetYScale()
    {
        float3 size = transform.GetComponent<Renderer>().bounds.size;
        float3 center = model.transform.position;
        float yDown = center.y - 0.5f * size.y;
        float yUp = center.y + 0.5f * size.y;
        float2 yScale = new float2(yDown, yUp);
        return yScale;
    }
    private float2 GetXScale()
    {
        float3 size = transform.transform.GetComponent<Renderer>().bounds.size;
        float3 center = model.transform.position;
        float xDown = center.x - 0.5f * size.x;
        float xUp = center.x + 0.5f * size.x;
        float2 xScale = new float2(xDown,xUp);
        return xScale;
    }
    private float2 GetZScale()
    {
        float3 size = transform.transform.GetComponent<Renderer>().bounds.size;
        float3 center = model.transform.position;
        float zDown = center.z - 0.5f * size.z;
        float zUp = center.z + 0.5f * size.z;
        float2 zScale = new float2(zDown, zUp);
        return zScale;
    }
    // 0.2����������Ϊ���ߵ���ʼ�����ֹ�������һ������
    private Vector3 GetRayOrigin()
    {
        float ranx = UnityEngine.Random.Range(GetXScale().x, GetXScale().y);
        float rany = UnityEngine.Random.Range(GetYScale().x, GetYScale().y);
        float ranz = UnityEngine.Random.Range(GetZScale().x, GetZScale().y);
        Vector3 origin = new Vector3(ranx, rany, ranz);
        return origin;
    }
    private Vector3 GetRayEndding()
    {
        float ranx = UnityEngine.Random.Range(GetXScale().x, GetXScale().y);
        float rany = UnityEngine.Random.Range(GetYScale().x, GetYScale().y);
        float ranz = UnityEngine.Random.Range(GetZScale().x, GetZScale().y);
        Vector3 ending = new Vector3(ranx, rany, ranz);
        return ending;
    }

    private void RandomRayMethod()
    {
        int method = UnityEngine.Random.Range(0, 3);
        
        switch (method)
        {
            case 0:
                GenerateRandomRayStableZ();
                Debug.Log("z");
                break;
            case 1:
                GenerateRandomRayStableY();
                Debug.Log("y");
                break;
            case 2:
                GenerateRandomRayStableX();
                Debug.Log("x");
                break;
        }

    }

    // 0.3����������������߷����������Ǹ�������Ǹ���ͽ���һ����ֹ�����꣩
    private void RandomRayDirection()
    {
        float random = UnityEngine.Random.value;
        if(random >= 0.5f)
        {
            // ����һ�����ߵ�������
            float3 temp = new float3();
            temp = origin;
            origin = endding;
            endding = temp;
            rayDirection = false;
            Debug.Log("����");
         }
        else
        {
            rayDirection = true;
            Debug.Log("����");
        }
    }

    private float3 LocalPosition()
    {
        float3 localPosition;
        float3 hitPoint = hit.point;
        float3 fatherSize = transform.GetComponent<Renderer>().bounds.size;
        float3 fatherPosition = transform.GetComponent<Renderer>().bounds.center;
        localPosition = (hitPoint - fatherPosition) / fatherSize;
        return localPosition;
    }

    // 0.4-2ɸѡ�����������ĵĵ㣨����ײ����ȡ���Ĺ�һ�����normal����ֵ���������ȴ���ĳһ��ֵ����ɹ����ѵ����������ţ�
    private bool IfNearCenter2()
    {

        if (Mathf.Abs(normalHitPoint.x) >= maxDistanceX || Mathf.Abs(normalHitPoint.z) >= maxDistanceZ)
        {
            return false;
        }
        else
        {
            return true;
        }


    }
    // 0.4-3ɸѡ�����������ĵĵ㣨����ײ����ȡ���Ĺ�һ�����normal����ֵ���������ȴ���ĳһ��ֵ����ɹ����ѵ����������ţ�
    private bool IfNearCenter3()
    {
        // ���㵽ת��ľ���
        float distance = Mathf.Sqrt(Mathf.Pow(normalHitPoint.x, 2.0f) + Mathf.Pow(normalHitPoint.z, 2.0f));
        Debug.Log("ת�����" + distance);
        if ( distance >= maxDistancePiot)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    // 1.Ϊ������ƽ�������������ֱ���ߵ�����ʼ��Ĳ������꣨����ƽ�����ĸ����Ǹ���������ֱ��ʹ�������޺����������ޣ�
    // 1.1��������
    private void GenerateRandomRayStableY()
    {
        origin = GetRayOrigin();
        endding = GetRayEndding();
        origin.y = GetYScale().y;
        endding.y = GetYScale().x;
        //endding.x = origin.x;
        //endding.z = origin.z;
    }
    // 1.2
    private void GenerateRandomRayStableX()
    {
        origin = GetRayOrigin();
        endding = GetRayEndding();
        origin.x = GetXScale().y;
        endding.x = GetXScale().x;
        // endding.y = origin.y;
        // endding.z = origin.z;
    }
    // 1.3 ��Զ����ҵ��������
    private void GenerateRandomRayStableZ()
    {
        origin = GetRayOrigin();
        endding = GetRayEndding();
        origin.z = GetZScale().x;
        endding.z = GetZScale().y;
        //endding.x = origin.x;
        //endding.y = origin.y;
    }



    // 2.��������
    private void StartRay()
    {
        // ���������ѡ��һ���������ߵķ�ʽ
        RandomRayMethod();
        // GenerateRandomRayStableY(model);
        // ������������ߵ�������
        RandomRayDirection();
        // ��������������
        ray = new Ray();
        ray.origin = origin;
        ray.direction = endding - origin;
        Debug.DrawRay(ray.origin, (endding - origin), Color.red);
    }

    // 3.���ɴ���
    public void GenerateMeshError()
    {
        nearCenter = true;

        while (nearCenter)
        {
            StartRay();
            while (!Physics.Raycast(ray, out hit, 100))
            {
                StartRay();
            }

            if (!hit.collider.CompareTag("3DError")&& !hit.collider.CompareTag("Box"))
            {
                // normalHitPoint = hit.point;
                normalHitPoint = LocalPosition();
                nearCenter = IfNearCenter3();
            }

        }

        // ���ɴ�����Object
        GameObject tderror = Instantiate(prefab3DError, transform);
        tderror.transform.parent = model.transform;
        tderror.transform.localScale /= 3; // set the scale of error

        // ��ȡ��ײλ����Ϣ
        tderror.transform.position = hit.point;
        tderror.transform.up = hit.normal;

        // ���ô�����Object
        tderror.transform.Translate(Vector3.up * 0.15f * tderror.transform.localScale.y, Space.Self);

        errors.Add(tderror);
    }


    public void GenerateMeshErrorInt(int errorNumber)
    {
        for (int i = 0; i < errorNumber; i++)
        {
            GenerateMeshError();
        }
        ifAddDone = true;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GenerateMeshErrorInt(2);
        }
    }
}
