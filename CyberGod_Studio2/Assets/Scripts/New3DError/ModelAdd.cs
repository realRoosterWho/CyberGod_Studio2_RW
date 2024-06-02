using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Burst.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.UI.Image;



public class ModelAdd : MonoBehaviour
{
    public List<GameObject> scaleCubes;
    public GameObject model;
    public GameObject prefab3DError;

    private RaycastHit hit = new RaycastHit();
    private Ray ray;
    [SerializeField] private List<GameObject> errors = new List<GameObject>();
    [SerializeField] private float sensitivity = 2f;

    private bool ifAddDone = false;

    // 取GameObject的前八个顶点（仅适用于Cube)
    private List<Vector3> GetWorldPositionOfVertexs(GameObject scaleCube)
    {
        return scaleCube.GetComponent<MeshFilter>()
            .sharedMesh
            .vertices
            .Select(v => scaleCube.transform.TransformPoint(v))
            .Take(8).ToList();
    }

    // 随机在cube表面取一点
    private Vector3 GetRandomVectorAtCube(List<Vector3> vec)
    {
        Vector3 res = new Vector3();
        // 随机一个法向量
        int randomStableAxis = Random.Range(0, 3);
        // 随机一个面的方向
        int randomAxisValue = Random.Range(0, 2);

        switch (randomStableAxis)
        {
            case 0:
                // x轴固定
                res.x = randomAxisValue == 0 ? vec[1].x : vec[0].x;
                res.y = Random.Range(Mathf.Min(vec[1].y, vec[3].y),
                    Mathf.Max(vec[1].y, vec[3].y));
                res.z = Random.Range(Mathf.Min(vec[3].z, vec[5].z),
                    Mathf.Max(vec[3].z, vec[5].z));
                break;
            case 1:
                // y轴固定
                res.x = Random.Range(Mathf.Min(vec[1].x, vec[0].x),
                    Mathf.Max(vec[1].x, vec[0].x));
                res.x = randomAxisValue == 0 ? vec[0].y : vec[2].y;
                res.z = Random.Range(Mathf.Min(vec[0].z, vec[6].z),
                    Mathf.Max(vec[0].z, vec[6].z));
                break;
            case 2:
                // z轴固定
                res.x = Random.Range(Mathf.Min(vec[1].x, vec[0].x),
                    Mathf.Max(vec[1].x, vec[0].x));
                res.y = Random.Range(Mathf.Min(vec[1].y, vec[3].y),
                    Mathf.Max(vec[1].y, vec[3].y));
                res.z = randomAxisValue == 0 ? vec[0].z : vec[4].z;
                break;
        }
        return res;
    }

    // 在指定部位生成射线
    private void StartRay(GameObject scaleCube)
    {
        List<Vector3> vec = GetWorldPositionOfVertexs(scaleCube);

        Vector3 origin = GetRandomVectorAtCube(vec);
        Vector3 end = scaleCube.transform.position;

        // 在这里生成射线
        ray = new Ray();
        ray.origin = origin;
        ray.direction = end - origin;
        // Debug.DrawRay(ray.origin, (end - origin), Color.red);
        // Debug.Log(myGameObject.transform.position);
    }

    // 一直生成射线直到击中模型
    public void hitWhileOnModel(GameObject scaleCube)
    {
        hitWhileOn(scaleCube);
        // 已经击中
        while (hit.collider.CompareTag("3DError") || hit.collider.CompareTag("Box") || hit.collider.CompareTag("ScaleCube"))
        {
            hitWhileOn(scaleCube);
        }
    }

    // 一直生成射线直到击中
    private void hitWhileOn(GameObject scaleCube)
    {
        StartRay(scaleCube);
        while (!Physics.Raycast(ray, out hit, 100))
        {
            StartRay(scaleCube);
        }
    }

    // 在指定部位生成错误
    private void GenerateMeshErrorAtArea(GameObject scaleCube)
    {
        hitWhileOnModel(scaleCube);
        Debug.Log(hit.collider.gameObject.name);

        float modelscale = model.transform.localScale.x;

        // 生成错误子Object
        GameObject tderror = Instantiate(prefab3DError, transform);
        errors.Add(tderror);
        tderror.transform.parent = model.transform;
        tderror.transform.localScale /= 3; // set the scale of error
        tderror.transform.localScale /= modelscale;

        // 获取碰撞位置信息
        tderror.transform.position = hit.point;
        tderror.transform.up = hit.normal;

        // 安置错误子Object
        tderror.transform.Translate(Vector3.up * 0.15f * tderror.transform.localScale.y, Space.Self);


    }
    private void Awake()
    {
        // 遍历所有scalebox，每个生成一个错误点
        for (int i = 0; i < scaleCubes.Count; i++)
        {
            GenerateMeshErrorAtArea(scaleCubes[i]);
            Debug.Log("DONE");
        }
        ifAddDone = true;

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
    // Update is called once per frame
    void Update()
    {
        if (ifAddDone)
        {
            float rotation = Input.GetAxis("Mouse X") * sensitivity;

            // 如果m_game_manager.Instance.isPause为真，返回
            /*
            if (m_GameManager.Instance.isPaused)
            {
                return;
            }
             */
            transform.Rotate(new Vector3(0, rotation, 0));
        }

        UpdateErrorList();
        var errornumber = GetErrorCount();

        // 当错误已经生成完毕 且 错误全部被消除
        if (errornumber <= 0 && ifAddDone )
        {
            //发生事件：SomethingRepaired
            EventManager.Instance.TriggerEvent("SomethingRepaired", new GameEventArgs());
            // Changeto Navigation Mode
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
<<<<<<< Updated upstream
            // 销毁自己的父物体（销毁的事啥子啊）
            Destroy(transform.root.gameObject,1f);
=======
            // 锟斤拷锟斤拷锟皆硷拷锟侥革拷锟斤拷锟藉（锟斤拷锟劫碉拷锟斤拷啥锟接帮拷锟斤拷
            Destroy(transform.parent.gameObject);
            
        }
        
        if (ControlMode_Manager.Instance.m_controlMode == ControlMode.DIALOGUE)
        {
            Debug.Log("Destroy");
            Destroy(transform.parent.gameObject);
>>>>>>> Stashed changes
        }

    }
}
