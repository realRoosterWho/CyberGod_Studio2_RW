using UnityEngine;
using UnityEngine.UI;

public class GeneralCountdown_Logic : MonoBehaviour
{
    [SerializeField]
    public Scrollbar countdownScrollbar;
    private ControlMode_Manager controlModeManager; // 添加这行代码


    private float maxCountdownTime;

    private void Start()
    {
        // Set the max value and current value of the slider to the max countdown time
        countdownScrollbar.size = 1;
        countdownScrollbar.value = 1;
        
        controlModeManager = ControlMode_Manager.Instance; // 添加这行代码
    }

    private void Update()
    {
        // 添加条件判断，只有在 ControlMode 为 NAVIGATION 或 REPAIRING 时才进行倒计时
        if (controlModeManager.m_controlMode == ControlMode.NAVIGATION || controlModeManager.m_controlMode == ControlMode.REPAIRING)
        {
            // Decrease the current value of the slider every frame to simulate the countdown
            countdownScrollbar.size -= Time.deltaTime / maxCountdownTime;
        }
    }
    
    public void SetMaxTime(float time)
    {
        maxCountdownTime = time;
    }
}