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
        // ���һ��������
        int randomStableAxis = Random.Range(0, 3);
        // ���һ����ķ���
        int randomAxisValue = Random.Range(0, 2);

        switch (randomStableAxis)
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

    // ��ָ����λ��������
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

    // һֱ��������ֱ������ģ��
    public void hitWhileOnModel(GameObject scaleCube)
    {
        hitWhileOn(scaleCube);
        // �Ѿ�����
        while (hit.collider.CompareTag("3DError") || hit.collider.CompareTag("Box") || hit.collider.CompareTag("ScaleCube"))
        {
            hitWhileOn(scaleCube);
        }
    }

    // һֱ��������ֱ������
    private void hitWhileOn(GameObject scaleCube)
    {
        StartRay(scaleCube);
        while (!Physics.Raycast(ray, out hit, 100))
        {
            StartRay(scaleCube);
        }
    }

    // ��ָ����λ���ɴ���
    private void GenerateMeshErrorAtArea(GameObject scaleCube)
    {
        hitWhileOnModel(scaleCube);
        Debug.Log(hit.collider.gameObject.name);

        float modelscale = model.transform.localScale.x;

        // ���ɴ�����Object
        GameObject tderror = Instantiate(prefab3DError, transform);
        errors.Add(tderror);
        tderror.transform.parent = model.transform;
        tderror.transform.localScale /= 3; // set the scale of error
        tderror.transform.localScale /= modelscale;

        // ��ȡ��ײλ����Ϣ
        tderror.transform.position = hit.point;
        tderror.transform.up = hit.normal;

        // ���ô�����Object
        tderror.transform.Translate(Vector3.up * 0.15f * tderror.transform.localScale.y, Space.Self);


    }
    private void Awake()
    {
        // ��������scalebox��ÿ������һ�������
        for (int i = 0; i < scaleCubes.Count; i++)
        {
            GenerateMeshErrorAtArea(scaleCubes[i]);
            // Debug.Log("DONE");
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

            // ���m_game_manager.Instance.isPauseΪ�棬����
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

        // �������Ѿ�������� �� ����ȫ��������
        if (errornumber <= 0 && ifAddDone )
        {
            //�����¼���SomethingRepaired
            EventManager.Instance.TriggerEvent("SomethingRepaired", new GameEventArgs());
            // Changeto Navigation Mode
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
            // �����Լ��ĸ����壨���ٵ���ɶ�Ӱ���
            Destroy(transform.parent.gameObject);
            
        }
        
        if (ControlMode_Manager.Instance.m_controlMode == ControlMode.DIALOGUE)
        {
            Destroy(transform.parent.gameObject);
        }

    }
}
