using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_MeshErrorGenerator : MonosingletonTemp<Generation_MeshErrorGenerator>
{
    [SerializeField]
    //获取一个Prefab叫做MeshError
    private List<GameObject> m_meshErrorPrefabList;

    private GameObject m_meshError;

    [SerializeField] private GameObject m_ErrorPos;
    
    //ObjectInfo Define
    public ObjectInfo m_meshinfo;
    public ObjectInfo m_rotateinfo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_meshError != null)
        {
            //如果Layer_Handler.Instance.m_layer = Layer.Flesh,发送m_meshinfo;如果Layer_Handler.Instance.m_layer = Layer.Machine,发送m_rotateinfo
            if (Layer_Handler.Instance.m_layer == Layer.FLESH)
            {
                UIDisplayManager.Instance.DisplayLeftInfo(m_meshinfo);
            }
            else if (Layer_Handler.Instance.m_layer == Layer.MACHINE)
            {
                UIDisplayManager.Instance.DisplayLeftInfo(m_rotateinfo);
            }
            
        }
    }
    
    //生成一个MeshError
    public void GenerateMeshError(int index)
    {

        
        //如果m_meshError不为空，Debug并且返回
        
        
        m_meshError = Instantiate(m_meshErrorPrefabList[index], m_ErrorPos.transform);
        m_meshError.transform.position = m_ErrorPos.transform.position;


    }
}
