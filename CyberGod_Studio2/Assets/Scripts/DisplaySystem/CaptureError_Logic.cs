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
        //获取m_ErrorPos，在自己的子物体里面找到名字是generationPos的物体
        m_ErrorPos = transform.Find("generationPos");
    }

    // Update is called once per frame
    void Update()
    {
        //Update Error Count
        m_errorCount = m_ErrorList.Count;
    }
    
    public void CaptureError()
    {
        if (m_ErrorPos == null)
        {
            m_ErrorPos = transform.Find("generationPos");
            if (m_ErrorPos == null)
            {
                throw new System.Exception("m_ErrorPos is still null after trying to initialize it");
                return;
            }
        }

        GameObject error = Instantiate(m_ErrorPrefab, m_ErrorPos);
        m_ErrorList.Add(error);
    }
    
    // void OnDestroy()
    // {
    //     EventManager.Instance.RemoveEvent("ErrorDestroyed", OnErrorDestroyed);
    // }
}
