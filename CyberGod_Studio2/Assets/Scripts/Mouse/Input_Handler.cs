using UnityEngine;
using UnityEngine.UI;

public class Input_Handler : MonoBehaviour
{
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
        // Lock the cursor to the center of the screen at start
        Cursor.lockState = CursorLockMode.Locked;

        // Subscribe to the MotionCapture_Input event
        EventManager.Instance.AddEvent("MotionCaptureInput", OnMotionCaptureInput);
    }

    void Update()
    {
        // Track mouse movement
        TrackMouseMovement();
        
        // Check if the mouse has moved the maximum distance
        CheckMaxDistance();
        
        // Change scrollbar value
        ChangeScrollbarValue();
        
        // Handle input locking
        InputLock();
    }

    private void InputLock()
    {
        // Toggle cursor lock state on ESC key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        }
        
        // Reset distances on left mouse button click
        if (Input.GetMouseButtonDown(0))
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
    }

    private void ChangeScrollbarValue()
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
}
