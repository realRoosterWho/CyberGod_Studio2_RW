using UnityEngine;
using UnityEngine.UI;

public class Input_Handler : MonoBehaviour
{
    
    private ControlMode m_controlMode = ControlMode.NAVIGATION;
    private RepairingSubMode m_repairingSubMode = RepairingSubMode.ERROR_REPAIR;
    private float distanceX = 0f;
    private float distanceY = 0f;
    private float XMAX = 500.0f;
    private float YMAX = 500.0f;

    // Reference to Health_Handler script
    [SerializeField] private Health_Handler m_healthHandler;

    [SerializeField] private Scrollbar m_scrollbar;
    [SerializeField] private Layer_Handler m_layerHandler;
    
    

    void Start()
    {

        // Subscribe to the MotionCapture_Input event
        EventManager.Instance.AddEvent("MotionCaptureInput", OnMotionCaptureInput);
        
        //监听三个ControlMode的事件
        EventManager.Instance.AddEvent("NavigationMode", OnIntoNavigationMode);
        EventManager.Instance.AddEvent("RepairingMode", OnIntoRepairingMode);
        EventManager.Instance.AddEvent("DialogueMode", OnIntoDialogueMode);
        EventManager.Instance.AddEvent("NormalMode", OnIntoNormalMode);
    }

    void Update()
    {   
        // Change scrollbar value
        DisplayScrollbarValue();
        UpdateModeAction();

        ChangeLayer();

        // Handle input locking
        // InputLock();
    }

    private void InputLock() //目前不适用
    {
        // Toggle cursor lock state on ESC key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    
    
    private void ChangeLayer()
    {
        //only work in Navigation Mode
        if (m_controlMode == ControlMode.NAVIGATION && Input.GetMouseButtonDown(1))
        {
            m_layerHandler.SwitchLayer();
            ResetDistances();
        }
    }

    private void TrackMouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Accumulate the absolute mouse displacement
        distanceX += Mathf.Abs(mouseX);
        distanceY += Mathf.Abs(mouseY);
    }

    private void ResetDistances()
    {
        distanceX = 0f;
        distanceY = 0f;
    }
    
    private void CheckMaxDistance()
    {
        if (distanceX >= XMAX || distanceY >= YMAX)
        {
            Repaired();
        }
    }
    
    private void Repaired()
    {
        // Call the ChangeRepair function from Health_Handler script with an input parameter of 10
        m_healthHandler.ChangeRepair(10.0f);
        
        // Reset distances
        ResetDistances();

		//发生事件：SomethingRepaired
		EventManager.Instance.TriggerEvent("SomethingRepaired", new GameEventArgs());
		// Changeto Navigation Mode
		ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
    }

    private void DisplayScrollbarValue()
    {
        float x_percent = distanceX / XMAX;
        float y_percent = distanceY / YMAX;
        
        m_scrollbar.value = x_percent;
    }

	private void OnMotionCaptureInput(GameEventArgs args)
    {
        // Get the Vector3 value from the GameEventArgs
       	float motionCaptureInput = args.FloatValue;
        
		// Debug.Log("Motion Capture Input: " + motionCaptureInput);
    }
        
    //定义四个切换ControlMode的函数
    private void OnIntoNavigationMode(GameEventArgs args)
    {
        m_controlMode = ControlMode.NAVIGATION;
    }
    
    private void OnIntoRepairingMode(GameEventArgs args)
    {
        m_controlMode = ControlMode.REPAIRING;
    }
    
    private void OnIntoDialogueMode(GameEventArgs args)
    {
        m_controlMode = ControlMode.DIALOGUE;
    }
    private void OnIntoNormalMode(GameEventArgs args)
    {
        m_controlMode = ControlMode.NORMAL;
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
    
    //Update函数，如果当前的m_controlMode = ControlMode，就执行对应的函数
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

    void UpdateRepairingModeAction()
    {
        m_repairingSubMode = ControlMode_Manager.Instance.m_repairingSubMode;
        switch (m_repairingSubMode)
        {
            case RepairingSubMode.ERROR_REPAIR:
                ErrorRepair();
                break;
            case RepairingSubMode.CLOCKWORK_REPAIR:
                ClockworkRepair();
                break;
        }
    }
    
    void ErrorRepair()
    {
        // Track mouse movement
        TrackMouseMovement();
        // Check if the mouse has moved the maximum distance
        CheckMaxDistance();
    }
    
    void ClockworkRepair()
    {
        // Debug.Log("Clockwork Repair");
    }
}
