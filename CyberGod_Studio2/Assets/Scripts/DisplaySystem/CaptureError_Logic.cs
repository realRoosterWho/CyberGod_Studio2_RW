using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CaptureError_Logic : MonoBehaviour
{
    [SerializeField] private GameObject m_ErrorPrefab;
    
    private List<GameObject> m_ErrorList = new List<GameObject>();
    
    [SerializeField]public int m_errorCount = 0;
    
    [SerializeField] private Transform m_ErrorPos;
    
    // Start is called before the first frame update
    void Start()
    {
        // CaptureError();
    }

    // Update is called once per frame
    void Update()
    {
        //Update Error Count
        m_errorCount = m_ErrorList.Count;
    }
    
    public void CaptureError()
    {
        GameObject error = Instantiate(m_ErrorPrefab, m_ErrorPos);
        m_ErrorList.Add(error);
    }
}
