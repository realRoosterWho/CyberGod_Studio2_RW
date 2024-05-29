using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum Layer
{
    FLESH,
    MACHINE,
    NERVE
}

public class Layer_Handler : MonosingletonTemp<Layer_Handler>
{
	//定义cinimachine impulse source
	public CinemachineImpulseSource m_impulseSource;
    
    //定义当前的层级
    [SerializeField] public Layer m_layer = Layer.FLESH;

	private ControlMode m_controlMode;
    
    // Start is called before the first frame update
    void Start()
    {
        //在自己身上获取CinemachineImpulseSource组件
		m_impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateModeAction();
    }
    
    //定义一个函数，用于指定地改变当前的层级
    public void ChangeLayer(Layer layer)
    {
        m_layer = layer;
    }
    //定义一个函数，用于按顺序切换当前的层级
    public void SwitchLayer()
    {
        m_layer = (Layer)(((int)m_layer + 1) % 3); //这里的3是Layer枚举类型的数量
    }

    //Update函数，如果当前的m_controlMode = ControlMode，就执行对应的函数
    public void UpdateModeAction()
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

	private void NavigationMode()
    {
        // Debug.Log("Navigation Mode");
        Cursor.lockState = CursorLockMode.Locked;
		ChangeLayer();
    }

	private void RepairingMode()
    {
        // Debug.Log("Repairing Mode");
	}

	private void DialogueMode()
    {
        // Debug.Log("Dialogue Mode");
    }

	private void NormalMode()
    {
        // Debug.Log("Normal Mode");
    }

    private void ChangeLayer()
    {
        //only work in Navigation Mode
        if (m_controlMode == ControlMode.NAVIGATION && Input.GetMouseButtonDown(1))
        {
            SoundManager.Instance.PlaySFX(0);
			m_impulseSource.GenerateImpulse(0.2f);
            SwitchLayer();
        }
    }

}
