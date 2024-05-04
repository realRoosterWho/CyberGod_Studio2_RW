using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStage_Handler : MonoBehaviour
{
    [SerializeField]Body_Manager m_bodyManager;
    
    private ControlMode m_controlMode;
    private RepairingSubMode m_repairingSubMode;
    
    [SerializeField] private Generation_MeshErrorGenerator m_meshErrorGenerator;
    
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
        UpdateModeAction();
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
            //抽一个随机布尔值，如果为true，就生成一个在Flesh，否则在Machine
            bool isFlesh = Random.Range(0, 2) == 0;
            if (isFlesh)
            {
                m_bodyManager.GenerateRandomError("Flesh");
            }
            else
            {
                m_bodyManager.GenerateRandomError("Machine");
            }
            m_generationTimer = 0.0f;
        }
    }
    
    
    
    
    
    
    
    
    
    void UpdateModeAction()
    {
        m_controlMode = ControlMode_Manager.Instance.m_controlMode;
        switch (m_controlMode)
        {
            case ControlMode.NAVIGATION:
                NavigationMode();
                break;
            case ControlMode.REPAIRING:
                RepairingMode();
                break;
            case ControlMode.DIALOGUE:
                DialogueMode();
                break;
            case ControlMode.NORMAL:
                NormalMode();
                break;
        }
    }
    //定义四个函数，用于执行在不同m_conotrolMode = ControlMode下的操作
    private void NavigationMode()
    {
        // Debug.Log("Navigation Mode");
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void RepairingMode()
    {
        // Debug.Log("Repairing Mode");
        Cursor.lockState = CursorLockMode.Locked;
        UpdateRepairingModeAction();

    }
    
    private void DialogueMode()
    {
        // Debug.Log("Dialogue Mode");
    }
    
    private void NormalMode()
    {
        // Debug.Log("Normal Mode");
    }
    
    void UpdateRepairingModeAction()
    {
        m_repairingSubMode = ControlMode_Manager.Instance.m_repairingSubMode;
        switch (m_repairingSubMode)
        {
            case RepairingSubMode.ERROR_REPAIR:
                MeshErrorGeneration();
                break;
            case RepairingSubMode.CLOCKWORK_REPAIR:
                break;
        }
    }
    
    void MeshErrorGeneration()
    {
        m_meshErrorGenerator.GenerateMeshError();
    }
}
