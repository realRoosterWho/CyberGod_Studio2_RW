using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStage_Handler : MonoBehaviour
{
    [SerializeField]Body_Manager m_bodyManager;
    
    float m_generationInterval = 10.0f;
	[SerializeField] float m_generationIntervalMin = 2.0f;
	[SerializeField] float m_generationIntervalMax = 5.0f;
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
		m_generationInterval = Random.Range(m_generationIntervalMin, m_generationIntervalMax);
		
        m_generationTimer += Time.deltaTime;
        if (m_generationTimer > m_generationInterval)
        {
            m_bodyManager.GenerateRandomError();
            m_generationTimer = 0.0f;
        }
    }
}
