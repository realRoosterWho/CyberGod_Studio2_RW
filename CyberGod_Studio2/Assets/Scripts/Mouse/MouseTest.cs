using UnityEngine;
using UnityEngine.UI;


public class MouseTest : MonoBehaviour
{
    private float distanceX = 0f;
    private float distanceY = 0f;

    private float XMAX = 500.0f;
    private float YMAX = 500.0f;
    //获取Health_Handler脚本
    [SerializeField] private Health_Handler m_healthHandler;

    [SerializeField] private Scrollbar m_scrollbar;
    
    [SerializeField] private Layer_Handler m_layerHandler;

    void Start()
    {
        // Lock the cursor to the center of the screen at start
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        // Toggle cursor lock state on ESC key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // Track mouse movement
        TrackMouseMovement();
        
        // Check if the mouse has moved the maximum distance
        CheckMaxDistance();
        
        //Change scrollbar value
        ChangeScrollbarValue();

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

        // Log the accumulated distances
        Debug.Log($"distanceX: {distanceX} distanceY: {distanceY}");
    }

    private void ResetDistances()
    {
        distanceX = 0f;
        distanceY = 0f;
    }
    
    //检测是否到达最大值
    private void CheckMaxDistance()
    {
        if (distanceX >= XMAX || distanceY >= YMAX)
        {
            Repaired();
        }
    }
    
    
    private void Repaired()
    {
        //调用Health_Handler脚本中的ChangeRepair函数，输入参数为10
        m_healthHandler.ChangeRepair(10.0f);
        
        //恢复距离
        ResetDistances();
    }

    private void ChangeScrollbarValue()
    {
        float x_percent = distanceX / XMAX;
        float y_percent = distanceY / YMAX;
        
        m_scrollbar.value = x_percent;
    }
}