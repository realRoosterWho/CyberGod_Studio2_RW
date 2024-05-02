using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPos_Logic : MonoBehaviour
{
    public enum BodyState //这是一个枚举类型，用于表示身体的状态
    {
        Active,
        Inactive,
		Repairing
    }
    
    public bool hasError = false;

    SpriteRenderer m_spriteRenderer;
    
    Layer m_layer = Layer.FLESH;
    RepairingSubMode m_repairingSubMode = RepairingSubMode.ERROR_REPAIR;

    [SerializeField]BodyState m_bodyState = BodyState.Inactive;
	[SerializeField]BodyState m_bodyStateInput = BodyState.Inactive;

    //获取Error预制体
    [SerializeField] private GameObject m_errorPrefab;
    private GameObject m_error = null;
    [SerializeField] private GameObject m_fleshlayer;
    [SerializeField] private GameObject m_mechaniclayer;
    [SerializeField] private GameObject m_nervelayer;
    
    private fleshlayer_Logic m_fleshlayer_Logic;
    private mechaniclayer_Logic m_mechaniclayer_Logic;
    private nervelayer_Logic m_nervelayer_Logic;

	//生成计时器
	private float m_Errortime = 0.0f;

	//生成计时器ActiveTimer
	private float m_ActiveTimer = 0.0f;

	const float ACTIVE_TIME = 1.0f;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        
        //get layerlogics
        m_fleshlayer_Logic = m_fleshlayer.GetComponent<fleshlayer_Logic>();
        m_mechaniclayer_Logic = m_mechaniclayer.GetComponent<mechaniclayer_Logic>();
        m_nervelayer_Logic = m_nervelayer.GetComponent<nervelayer_Logic>();

		EventManager.Instance.AddEvent("SomethingRepaired", OnSomethingRepaired);
		
		
		//for test
		// GenerateError();
    }

    void Update()
    {
        UpdateBodyStateBehavior();//根据不同的控制状态，执行不同的操作

        UpdateLayer();
        
        
        UpdatehasError();//判断是否有Error，用作状态显示
		UpdateErrorTimeCounting();//计时Error存在了多久，不过目前还没有用
		UpdateActiveTimer();//计时当前Active了多久
		CheckIntoRepair();//检查是否需要进入Repair状态
    }
    public void ChangeColor(Color color)
    {
        m_spriteRenderer.color = color;
    }

	//改变透明度
	    public void ChangeAlpha(float alpha)
    {
        Color color = m_spriteRenderer.color;
        color.a = alpha;
        m_spriteRenderer.color = color;
    }

    public void BodyStateChange(string state)
    {
        //将字符串转换为枚举类型
        m_bodyState = (BodyState)System.Enum.Parse(typeof(BodyState), state);
    }
    
    public void GenerateError()
    {
        //生成一个Error，使之成为m_fleshlayer的子物体
        m_error = Instantiate(m_errorPrefab, m_fleshlayer.transform);
        
        //设置Error的位置与m_flshlayer的位置一致
        m_error.transform.position = m_fleshlayer.transform.position;
        
    }
    
    
    public void OnBodyStateActive()
    {
        //将颜色改成十六进制992514
		ChangeColor(new Color(0.6f, 0.145f, 0.081f));

    }
    
    public void OnBodyStateInactive()
    {
        ChangeColor(Color.white);
		//透明度改为0.6
		ChangeAlpha(0.3f);
    }

	public void OnBodyStateRepairing()
	{
	    ChangeColor(Color.green);

	    OnSubRepairingMode();
	}

    public void UpdateBodyStateBehavior()
    {
        // Debug.Log("UpdateBodyStateBehavior");

        switch (m_bodyState) //根据不同的状态，执行不同的操作
        {
            case BodyState.Active:
                OnBodyStateActive();
                break;
            case BodyState.Inactive:
                OnBodyStateInactive();
                break;
			case BodyState.Repairing:
			    OnBodyStateRepairing();
			    break;
        }
    }
    
    public void UpdatehasError()
    {
        if (m_error != null)
        {
            hasError = true;
        }
        else
        {
            hasError = false;
        }
    }

	public void UpdateErrorTimeCounting()
	{
	    if (hasError)
        {
            m_Errortime += Time.deltaTime;
        }
		else
		{
		    m_Errortime = 0.0f;
		}
    }
	
	//ActiveTimer计时器
	public void UpdateActiveTimer()
    {
        if (m_bodyState == BodyState.Active)
        {
            m_ActiveTimer += Time.deltaTime;
        }
        else
        {
            m_ActiveTimer = 0.0f;
        }
    }

	//定义函数，用于销毁Error
    public void DestroyError()
    {
		//如果我有Error我摧毁Error，如果没有我Debug
		if (m_error != null)
        {
            m_error.GetComponent<ErrorLogic>().DestroyError();
        }
        else
        {
            Debug.Log("No error to destroy");
        }
    }
    

	//判断，如果有Error且ActiveTimer大于Destroytime
    public void CheckIntoRepair()
    {
	    m_repairingSubMode = ControlMode_Manager.Instance.m_repairingSubMode;
	    //针对不同的RepairingSubMode，进行不同的操作
	    switch (m_repairingSubMode)
	    {
		    case RepairingSubMode.ERROR_REPAIR:
			    if (hasError && m_ActiveTimer > ACTIVE_TIME)
			    {
				    //进入Repair状态
				    ControlMode_Manager.Instance.ChangeControlMode(ControlMode.REPAIRING);
				    //我自己也进入Repair状态
				    m_bodyState = BodyState.Repairing;
			    }
			    break;
		    
		    case RepairingSubMode.CLOCKWORK_REPAIR:
			    if (m_mechaniclayer_Logic.hasClockwork && m_ActiveTimer > ACTIVE_TIME)
			    {
				    //进入Repair状态
				    ControlMode_Manager.Instance.ChangeControlMode(ControlMode.REPAIRING);
				    //我自己也进入Repair状态
				    m_bodyState = BodyState.Repairing;
			    }
			    break;
	    }
    }
    
    public void CheckOutofRepair()
	{
		//如果我自己在Repair状态，但是Mode是NAVIGATION，那么我就进入Inactive状态
		if (m_bodyState == BodyState.Repairing && ControlMode_Manager.Instance.m_controlMode == ControlMode.NAVIGATION)
		{
			m_bodyState = BodyState.Inactive;
		}
	}

	//当有东西被修复时
	public void OnSomethingRepaired(GameEventArgs args)
    {
		//如果我自己在Repair状态，那么我就进入Inactive状态，并且我摧毁我的Error
		if (m_bodyState == BodyState.Repairing)
		{
		    switch (m_repairingSubMode)
		    {
			    case RepairingSubMode.ERROR_REPAIR:
				    DestroyError();
				    m_bodyState = BodyState.Inactive;
				    ControlMode_Manager.Instance.ChangeControlMode(ControlMode.NAVIGATION);
				    break;
			    case RepairingSubMode.CLOCKWORK_REPAIR:
				    ClockworkInput();
				    break;
		    }
		}
    }

	public void ClockworkInput()
	{
		//检查是否有Clockwork
		if (!m_mechaniclayer_Logic.hasClockwork)
		{
			return;
		}
		
		m_mechaniclayer_Logic.HandleClockworkInput();
	}
	
	//写一个UpdateOnSubRepairingMode,在BodyState为Repairing的时候，调用这个函数，针对不同的SubMode进行不同的操作
	public void OnSubRepairingMode()
	{
		m_repairingSubMode = ControlMode_Manager.Instance.m_repairingSubMode;
		switch (m_repairingSubMode)
		{
			case RepairingSubMode.ERROR_REPAIR:
				break;
			case RepairingSubMode.CLOCKWORK_REPAIR:
				m_mechaniclayer_Logic.ClockworkRepairing();
				break;
		}
	}
	
	
	void EnableRenderers(Renderer[] renderers, bool enabled)
	{
		foreach (var renderer in renderers)
		{
			renderer.enabled = enabled;
		}
	}

	public void UpdateLayer()
	{
		m_layer = Layer_Handler.Instance.m_layer;
		m_repairingSubMode = ControlMode_Manager.Instance.m_repairingSubMode;
		UpdateLayerDisplay();
		
		//当是对应的层级的时候，打开SpriteRenderer，否则关闭
		switch (m_layer)
		{
			case Layer.FLESH:
				OnFleshLayer();
				break;
			case Layer.MACHINE:
				OnMachineLayer();
				break;
			case Layer.NERVE:
				OnNerveLayer();
				break;
		}
	}
	
	private void OnFleshLayer()
	{
		//更新submode为Error
		ControlMode_Manager.Instance.m_repairingSubMode = RepairingSubMode.ERROR_REPAIR;
	}
	
	private void OnMachineLayer()
	{
		//更新submode为Clockwork
		ControlMode_Manager.Instance.m_repairingSubMode = RepairingSubMode.CLOCKWORK_REPAIR;
	}
	
	private void OnNerveLayer()
	{

	}

	public void UpdateLayerDisplay()
	{
		m_layer = Layer_Handler.Instance.m_layer;

		// 获取m_fleshlayer, m_mechaniclayer, m_nervelayer及其子物体的所有Renderer
		Renderer[] fleshlayerRenderers = m_fleshlayer.GetComponentsInChildren<Renderer>();
		Renderer[] mechaniclayerRenderers = m_mechaniclayer.GetComponentsInChildren<Renderer>();
		Renderer[] nervelayerRenderers = m_nervelayer.GetComponentsInChildren<Renderer>();

		// 当是对应的层级的时候，打开Renderer，否则关闭
		switch (m_layer)
		{
			case Layer.FLESH:
				EnableRenderers(fleshlayerRenderers, true);
				EnableRenderers(mechaniclayerRenderers, false);
				EnableRenderers(nervelayerRenderers, false);
				break;
			case Layer.MACHINE:
				EnableRenderers(fleshlayerRenderers, false);
				EnableRenderers(mechaniclayerRenderers, true);
				EnableRenderers(nervelayerRenderers, false);
				break;
			case Layer.NERVE:
				EnableRenderers(fleshlayerRenderers, false);
				EnableRenderers(mechaniclayerRenderers, false);
				EnableRenderers(nervelayerRenderers, true);
				break;
		}
	}
}
