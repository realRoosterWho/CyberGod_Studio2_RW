using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mechaniclayer_Logic : MonoBehaviour
{
    

    
    
    
    public ObjectInfo info;

    [SerializeField]public bool isActivated = false;
    
    
    
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && ControlMode_Manager.Instance.m_controlMode != ControlMode.REPAIRING)
        {
            UIDisplayManager.Instance.DisplayLeftInfo(info);
            DialogueManager.Instance.RequestSpiritSpeakEntry("mechanic");

        }
    }
    

}
