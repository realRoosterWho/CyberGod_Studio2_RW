using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMeshErrorParticleSystem : MonoBehaviour
{
    private GameObject m_parent;

    // Start is called before the first frame update
    void Start()
    {
        m_parent = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.identity;
    }
}