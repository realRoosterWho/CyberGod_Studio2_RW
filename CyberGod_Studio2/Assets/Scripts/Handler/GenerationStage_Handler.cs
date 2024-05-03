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

    private float MINIMAL_INTERVAL = 0.1f;

    [SerializeField]private bool m_isRandomTime;
    [SerializeField]private int m_maxNumber;
    
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
        
        GenerateError(m_isRandomTime, m_maxNumber);
        
        
    }
    
    public void GenerateError(bool isRandomTime = false, float maxNumber = 0.0f)
    {
        //获取bodymanager的错误数量
        int errorNumber = m_bodyManager.GetErrorNumber();
        Debug.Log("Error number: " + errorNumber);

        if (errorNumber >= maxNumber)
        {
            Debug.Log("Error number is full");
            return;
        }

        m_generationTimer += Time.deltaTime;

        if (isRandomTime)
        {
            m_generationInterval = Random.Range(m_generationIntervalMin, m_generationIntervalMax);
            if (m_generationTimer > m_generationInterval)
            {
                // m_bodyManager.GenerateRandomError();
                m_generationTimer = 0.0f;
            }
            return;
        }

        if (m_generationTimer > MINIMAL_INTERVAL)
        {
            m_bodyManager.GenerateRandomError();
            m_generationTimer = 0.0f;
        }
    }
}
