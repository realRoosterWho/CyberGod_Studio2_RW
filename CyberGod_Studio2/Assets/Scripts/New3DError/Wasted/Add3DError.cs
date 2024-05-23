#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; // 使用UnityEngine.Rendering.PostProcessing，而不是UnityEditor.Rendering.PostProcessing
using System.Net.NetworkInformation;
// using UnityEditor.PackageManager;
using System.Collections.Generic; // Add this to use List
using Unity.Mathematics;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.PackageManager;
using System.ComponentModel;
using System.Threading;


public class Add3DError : MonoBehaviour
{

    Ray ray; // random ray to hit neko generating the position of error
    public GameObject prefab3DError;// prefab of error

    public GameObject model; // 模型
    public RaycastHit hit;
    public float3 origin = new float3();
    public float3 endding = new float3();

    // 【检修语句】观察射线方向
    private bool rayDirection = true;

    // 用来判断撞击点是否太过靠近转轴的存储撞击点归一化后坐标的变量
    [SerializeField] private float3 normalHitPoint = new float3();
    // 在这里设定距离中心点的下限，生成的错误小于这个值统统不要（别太大容易死）
    [SerializeField] private float maxDistance = 0.85f;
    // 在这里设定距离转轴的下限，生成的错误小于这个值统统不要（别太大容易死）
    [SerializeField] private float maxDistanceX = 0.2f;
    [SerializeField] private float maxDistanceZ = 0.2f;
    // 在这里设定距离转轴的下限，生成的错误小于这个值统统不要（别太大容易死）
    [SerializeField] private float maxDistancePiot = 0.4f;
    private bool nearCenter = true;
    [SerializeField] private List<GameObject> errors = new List<GameObject>();

    static public bool ifAddDone = false;


    // 0.辅助函数
    // 0.1获取生成射线区域的xyz值上下限
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
    // 0.2依据上下限为射线的起始点和终止点随机出一个坐标
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

    // 0.3利用随机数决定射线方向是正向还是负向（如果是负向就交换一下起止点坐标）
    private void RandomRayDirection()
    {
        float random = UnityEngine.Random.value;
        if(random >= 0.5f)
        {
            // 更改一次射线的正负向
            float3 temp = new float3();
            temp = origin;
            origin = endding;
            endding = temp;
            rayDirection = false;
            Debug.Log("负向");
         }
        else
        {
            rayDirection = true;
            Debug.Log("正向");
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

    // 0.4-2筛选过于贴近中心的点（比如撞击点取到的归一化后的normal坐标值的向量长度大于某一个值才算成功，难道我真的是天才）
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
    // 0.4-3筛选过于贴近中心的点（比如撞击点取到的归一化后的normal坐标值的向量长度大于某一个值才算成功，难道我真的是天才）
    private bool IfNearCenter3()
    {
        // 计算到转轴的距离
        float distance = Mathf.Sqrt(Mathf.Pow(normalHitPoint.x, 2.0f) + Mathf.Pow(normalHitPoint.z, 2.0f));
        Debug.Log("转轴距离" + distance);
        if ( distance >= maxDistancePiot)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    // 1.为了生成平行于三个轴的竖直射线调整起始点的部分坐标（射线平行于哪个轴那个轴的坐标就直接使用上下限函数的上下限）
    // 1.1从上往下
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
    // 1.3 从远离玩家到靠近玩家
    private void GenerateRandomRayStableZ()
    {
        origin = GetRayOrigin();
        endding = GetRayEndding();
        origin.z = GetZScale().x;
        endding.z = GetZScale().y;
        //endding.x = origin.x;
        //endding.y = origin.y;
    }



    // 2.生成射线
    private void StartRay()
    {
        // 在这里随机选择一个生成射线的方式
        RandomRayMethod();
        // GenerateRandomRayStableY(model);
        // 在这里随机射线的正负向
        RandomRayDirection();
        // 在这里生成射线
        ray = new Ray();
        ray.origin = origin;
        ray.direction = endding - origin;
        Debug.DrawRay(ray.origin, (endding - origin), Color.red);
    }

    // 3.生成错误
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

        // 生成错误子Object
        GameObject tderror = Instantiate(prefab3DError, transform);
        tderror.transform.parent = model.transform;
        tderror.transform.localScale /= 3; // set the scale of error

        // 获取碰撞位置信息
        tderror.transform.position = hit.point;
        tderror.transform.up = hit.normal;

        // 安置错误子Object
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
