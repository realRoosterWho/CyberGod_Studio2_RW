using UnityEngine;
using System.Collections;
using UnityEditor.Rendering.PostProcessing;
using System.Net.NetworkInformation;
using UnityEditor.PackageManager;
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


    [SerializeField] public int errorNumber = 2;


    
    
    void Awake()
    {
        //生成指定数量错误
        GenerateMeshError(2);

    }
    void Update()
    {
        // show the hitting point when press W
        if (Input.GetKeyDown(KeyCode.W))
        {
            GenerateMeshError();
        }

        UpdateErrorList();
        
        var errornumber = GetErrorCount();
        if (errornumber <= 0)
        {
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
        float centerX = nekoCenter.x;
        float centerY = nekoCenter.y;
        float centerZ = nekoCenter.z;
        int a = 2;//box's length

        // random origin of the ray
        int ranx = Random.Range((int)nekoCenter.x - a, (int)nekoCenter.x + a);
        int rany = Random.Range((int)nekoCenter.y - a, (int)nekoCenter.y + a);
        int ranz = Random.Range((int)nekoCenter.z - a, (int)nekoCenter.z + a);

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
    public void GenerateMeshError(int errorNumber)
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

