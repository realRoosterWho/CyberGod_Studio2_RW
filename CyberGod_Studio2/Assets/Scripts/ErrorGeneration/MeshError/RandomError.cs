#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; // 使用UnityEngine.Rendering.PostProcessing，而不是UnityEditor.Rendering.PostProcessing
using System.Net.NetworkInformation;
// using UnityEditor.PackageManager;
using System.Collections.Generic; // Add this to use List

public class RandomError : MonoBehaviour
{
    Ray ray; // random ray to hit neko generating the position of error
    public GameObject prefab3DError;// prefab of error
    public GameObject neko; // neko
    private Vector3 nekoCenter; // store neko's center
    private Vector3 origin;// ray's origin
    private Vector3 end;// ray's end
    [SerializeField]private List<GameObject> errors = new List<GameObject>();
    
    //定义计时器
    [SerializeField] public float timer = 0;
    //定义最小存在时间
    [SerializeField] public float minTime = 3.0f;
    
    //定义错误开始生成时间
    [SerializeField] public float startTime = 0.5f;
    bool isStart = false;


    [SerializeField] public int errorNumber = 2;


    
    
    void Awake()
    {


    }
    
    void Start()
    {
        //获取自己名为default的子物体作为neko
        // neko = transform.Find("default").gameObject;

    }
    
    
    void Update()
    {
        //更新计时器
        timer += Time.deltaTime;
        
        // show the hitting point when press W
        if (Input.GetKeyDown(KeyCode.W))
        {
            GenerateMeshError();
        }
        
        //如果计时器大于开始时间，就开始生成错误
        if (timer > startTime && !isStart)
        {
            GenerateMeshErrorInt(errorNumber);
            isStart = true;
        }

        UpdateErrorList();
        
        var errornumber = GetErrorCount();
        if (errornumber <= 0 && timer > minTime)
        {
            
            Debug.Log("Suicide");
            //发生事件：SomethingRepaired
            EventManager.Instance.TriggerEvent("SomethingRepaired", new GameEventArgs());
            // Changeto Navigation Mode
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
            //销毁自己的父物体
            Destroy(transform.parent.gameObject);
        }
    }
    
    private void RandomRay()
    {
        // ray's origin box
        nekoCenter = neko.transform.position;// get neko's center
        //获取我自己的中心
        Vector3 myposition = transform.position;
        nekoCenter = myposition;
        
        float centerX = myposition.x;
        float centerY = myposition.y;
        float centerZ = myposition.z;
        int a = 2;//box's length

        // random origin of the ray
        int ranx = Random.Range((int)myposition.x - a, (int)myposition.x + a);
        int rany = Random.Range((int)myposition.y - a, (int)myposition.y + a);
        int ranz = Random.Range((int)myposition.z - a, (int)myposition.z + a);

        // inite the ray
        ray = new Ray();

        // calculate its end and origin
        origin = new Vector3(ranx, rany, ranz);
        end = nekoCenter;

        // inite its end and origin
        ray.origin = origin;
        ray.direction = end - origin;
    }
    public void GenerateMeshError()
    {
        // create random ray
        RandomRay();
        // draw the random ray
        Debug.DrawRay(ray.origin, (end - origin), Color.red);
        // inite the hit
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            // draw the ray
            Debug.DrawLine(ray.origin, hit.point);
            //如果碰撞到了的东西标签是3Dmodel才继续，否则返回并且重新调用一次GenerateMeshError
            if (hit.collider.tag != "3Dmodel")
            {
                GenerateMeshError();
                return;
            }

            //create a son object error from prefab
            GameObject tderror = Instantiate(prefab3DError, transform);
            tderror.transform.localScale /= 2; // set the scale of error

            // get the hitting imformation
            tderror.transform.position = hit.point;
            tderror.transform.up = hit.normal;

            // settle the error sphere at the surface of neko
            tderror.transform.Translate(Vector3.up * 0.5f * tderror.transform.localScale.y, Space.Self);
            
            // Add the new error to the list
            errors.Add(tderror);
        }
    }
    
    //生成指定数量个错误
    public void GenerateMeshErrorInt(int errorNumber)
    {
        for (int i = 0; i < errorNumber; i++)
        {
            GenerateMeshError();
        }
    }

    public int GetErrorCount()
    {
        return errors.Count;
    }
    
    public void UpdateErrorList()
    {
        // Check each error in the list
        for (int i = errors.Count - 1; i >= 0; i--)
        {
            // If the error is null, remove it from the list
            if (errors[i] == null)
            {
                errors.RemoveAt(i);
            }
        }
    }
}

