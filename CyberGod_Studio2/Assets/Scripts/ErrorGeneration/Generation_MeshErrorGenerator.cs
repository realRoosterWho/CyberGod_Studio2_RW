using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation_MeshErrorGenerator : MonoBehaviour
{
    [SerializeField]
    //获取一个Prefab叫做MeshError
    private GameObject m_meshErrorPrefab;

    private GameObject m_meshError;

    [SerializeField] private GameObject m_ErrorPos;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //生成一个MeshError
    public void GenerateMeshError()
    {
        if (m_meshError != null)
        {
            return;
        }
        
        m_meshError = Instantiate(m_meshErrorPrefab, m_ErrorPos.transform);
        m_meshError.transform.position = m_ErrorPos.transform.position;


    }
}
