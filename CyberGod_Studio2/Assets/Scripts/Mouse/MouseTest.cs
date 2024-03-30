using UnityEngine;
using UnityEngine.UI;


public class MouseTest : MonoBehaviour
{
    private float distanceX = 0f;
    private float distanceY = 0f;

    private float XMAX = 500.0f;
    private float YMAX = 500.0f;

    [SerializeField]
    private Scrollbar m_scrollbar;

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
        
        //Change scrollbar value
        ChangeScrollbarValue();

        // Reset distances on left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
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

    private void ChangeScrollbarValue()
    {
        float x_percent = distanceX / XMAX;
        float y_percent = distanceY / YMAX;
        
        m_scrollbar.value = x_percent;
    }
}