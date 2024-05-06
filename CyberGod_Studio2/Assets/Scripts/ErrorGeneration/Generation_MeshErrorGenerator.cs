using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_MeshErrorGenerator : MonoBehaviour
{
    [SerializeField]
    //获取一个Prefab叫做MeshError
    private List<GameObject> m_meshErrorPrefabList;

    private GameObject m_meshError;

    [SerializeField] private GameObject m_ErrorPos;
    
    //ObjectInfo Define
    public ObjectInfo m_meshinfo;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //生成一个MeshError
    public void GenerateMeshError(int index)
    {
        if (m_meshError != null)
        {
            //向UIManager发送m_meshinfo
            UIDisplayManager.Instance.DisplayLeftInfo(m_meshinfo);
            
            return;
        }
        
        //如果m_meshError不为空，Debug并且返回
        
        
        m_meshError = Instantiate(m_meshErrorPrefabList[index], m_ErrorPos.transform);
        m_meshError.transform.position = m_ErrorPos.transform.position;


    }
}
