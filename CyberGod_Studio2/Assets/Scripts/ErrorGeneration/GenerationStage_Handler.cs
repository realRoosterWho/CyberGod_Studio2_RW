using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStage_Handler : MonoBehaviour
{
    private ControlMode m_controlMode;
    private RepairingSubMode m_repairingSubMode;
    //存储已经有的错误部位
    private List<string> errorBodyParts_Flesh;
    private List<string> errorBodyParts_Machine;
    
    float m_generationInterval = 10.0f;
    private float m_generationIntervalMin;
    private float m_generationIntervalMax;
    private float m_generationTimer = 0.0f;

    private float MINIMAL_INTERVAL = 0.1f;
    
    
    
    
    
    
    
    [Header("References")]
    [SerializeField]Body_Manager m_bodyManager;
    [SerializeField] GeneralCountdown_Logic m_generalCountdownLogic;
    
    [SerializeField] private Generation_MeshErrorGenerator m_meshErrorGenerator;
    

    
    [SerializeField] CaptureError_Logic m_captureErrorLogic;
    [Space(10)] // 添加 10 像素的间隔
    
    [Header("步调值")]
    [SerializeField] int m_objectiveErrorNumber;
    [SerializeField] float m_maxCountdownTime;

    [SerializeField] private bool m_isDoubleError;
    // 新增的字段
    [SerializeField] List<string> realErrorGeneratableBodyParts_Flesh;
    [SerializeField] List<string> realErrorGeneratableBodyParts_Machine;

    

    

    [Space(10)] // 添加 10 像素的间隔
    
    
    
    [Header("遗产设置")]
    [SerializeField] float m_generationInterval_expectation = 5f;
    [SerializeField] float m_generationInterval_variance = 1.5f;

    [SerializeField]private bool m_isRandomTime;
    [SerializeField]private int m_maxNumber;
    
    // Start is called before the first frame update
    void Start()
    {
        //计算m_generationIntervalMin和m_generationIntervalMax,通过m_generationInterval_expectation和m_generationInterval_variance,正态分布
        m_generationIntervalMin = m_generationInterval_expectation - m_generationInterval_variance;
        m_generationIntervalMax = m_generationInterval_expectation + m_generationInterval_variance;
        
        //ErrorDestroyed事件订阅
        EventManager.Instance.AddEvent("ErrorDestroyed", OnErrorDestroyed);
        
        //设置倒计时的最大时间
        m_generalCountdownLogic.SetMaxTime(m_maxCountdownTime);
        
        // 使用新的列表替换原有的列表
        m_bodyManager.errorGeneratableBodyParts_Flesh = realErrorGeneratableBodyParts_Flesh;
        m_bodyManager.errorGeneratableBodyParts_Machine = realErrorGeneratableBodyParts_Machine;
        
    }

    // Update is called once per frame
    void Update()
    {
        //更新已经有的错误部位的列表
        errorBodyParts_Flesh = m_bodyManager.errorBodyParts_Flesh;
        errorBodyParts_Machine = m_bodyManager.errorBodyParts_Machine;
        
        UpdateModeAction();
        
        if (!m_isDoubleError)
        {
            GenerateError(m_isRandomTime, m_maxNumber);
        }
        else
        {
            
        }
        CheckEndGame();
        
        
        UpdateScoreUI(m_captureErrorLogic.m_errorCount, m_objectiveErrorNumber);


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

        
        //如果errorBodyParts_Flesh不为空
        if (errorBodyParts_Flesh.Count > 0)
        {
            //获取第一个元素
            var bodyPart = errorBodyParts_Flesh[0];
            //如果这个元素是"1"
            if (bodyPart == "1")
            {
                Debug.Log("GenerateMeshError"+bodyPart);
                m_meshErrorGenerator.GenerateMeshError(0);
            }
            //如果这个元素是"25"
            else if (bodyPart == "20")
            {
                Debug.Log("GenerateMeshError"+bodyPart);

                m_meshErrorGenerator.GenerateMeshError(1);
            }
            //如果这个元素是"26"
            else if (bodyPart == "25")
            {
                Debug.Log("GenerateMeshError"+bodyPart);

                m_meshErrorGenerator.GenerateMeshError(2);
            }
        }
        
        //如果errorBodyParts_Machine不为空
        if (errorBodyParts_Machine.Count > 0)
        {
            //获取第一个元素
            var bodyPart = errorBodyParts_Machine[0];
            //如果这个元素是"21"
            if (bodyPart == "21")
            {
                Debug.Log("GenerateMeshError"+bodyPart);

                m_meshErrorGenerator.GenerateMeshError(3);
            }
        }
        
        
    }
    
    void OnErrorDestroyed(GameEventArgs args)
    {
        m_captureErrorLogic.CaptureError();
    }
    
    void CheckEndGame()
    {
        if (m_captureErrorLogic.m_errorCount >= m_objectiveErrorNumber)
        {
            
            EventManager.Instance.TriggerEvent("OnWinning", new GameEventArgs());
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.DIALOGUE);
            // m_GameManager.Instance.ChangeScene("Win");
        }
        
        if (m_generalCountdownLogic.countdownScrollbar.size <= 0)
        {
            EventManager.Instance.TriggerEvent("OnLosing", new GameEventArgs());
            ControlMode_Manager.Instance.ChangeControlMode(ControlMode.DIALOGUE);
            m_GameManager.Instance.GameOver(m_captureErrorLogic.m_errorCount.ToString() + "/" + m_objectiveErrorNumber.ToString());
        }
    }

    void UpdateScoreUI(int errorNumber, int maxErrorNumber)
    {
        UIDisplayManager.Instance.DisplayScore(errorNumber, maxErrorNumber);
        
    }
}
