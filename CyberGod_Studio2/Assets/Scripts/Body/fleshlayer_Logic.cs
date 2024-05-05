using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fleshlayer_Logic : MonoBehaviour
{
    [SerializeField]public bool isActivated = false;
    public ObjectInfo info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            UIDisplayManager.Instance.DisplayInfo(info);
        }
        
    }
    
    
}
