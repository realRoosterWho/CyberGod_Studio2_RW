using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStage_Handler : MonoBehaviour
{
    [SerializeField]Body_Manager m_bodyManager;
    
    float m_generationInterval = 10.0f;
    
    [SerializeField] float m_generationInterval_expectation = 5f;
    [SerializeField] float m_generationInterval_variance = 1.5f;
    private float m_generationIntervalMin;
    private float m_generationIntervalMax;
    private float m_generationTimer = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        //计算m_generationIntervalMin和m_generationIntervalMax,通过m_generationInterval_expectation和m_generationInterval_variance,正态分布
        m_generationIntervalMin = m_generationInterval_expectation - m_generationInterval_variance;
        m_generationIntervalMax = m_generationInterval_expectation + m_generationInterval_variance;
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
