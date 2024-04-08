using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStage_Handler : MonoBehaviour
{
    [SerializeField]Body_Manager m_bodyManager;
    
    const float GENERATION_INTERVAL = 5.0f;
    private float m_generationTimer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GenerateRandomError();
    }
    
    //定义一个函数，用于生成错误
    public void GenerateRandomError()
    {
        m_generationTimer += Time.deltaTime;
        if (m_generationTimer > GENERATION_INTERVAL)
        {
            m_bodyManager.GenerateRandomError();
            m_generationTimer = 0.0f;
        }
    }
}
