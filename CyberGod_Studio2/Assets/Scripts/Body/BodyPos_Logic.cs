using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BodyPos_Logic : MonoBehaviour
{
    public enum BodyState //这是一个枚举类型，用于表示身体的状态
    {
        Active,
        Inactive,
    }
    
    public bool hasError = false;
    public bool hasError_Flesh = false;
    public bool hasError_Machine = false;
    
    
    
    public bool m_canRender = true;
    
    public bool m_canRender_Flesh = true;
    public bool m_canRender_Machine = true;
    public bool m_canRender_Nerve = true;
    
	public bool m_isRepairing = false;
    SpriteRenderer m_spriteRenderer;
    private GameObject m_Body_Manager;
    
    Layer m_layer = Layer.FLESH;
    RepairingSubMode m_repairingSubMode = RepairingSubMode.ERROR_REPAIR;

    [SerializeField]BodyState m_bodyState = BodyState.Inactive;
    //获取Error预制体
    [SerializeField] private GameObject m_errorPrefab;
    private GameObject m_error_Flesh = null;
    private GameObject m_error_Machine = null;

    [SerializeField] private GameObject m_fleshlayer;
    [SerializeField] private GameObject m_mechaniclayer;
    [SerializeField] private GameObject m_nervelayer;
    
    private fleshlayer_Logic m_fleshlayer_Logic;
    private mechaniclayer_Logic m_mechaniclayer_Logic;
    private nervelayer_Logic m_nervelayer_Logic;

    [SerializeField]private string m_bodynumber;

    
    
    
    
    
	//生成计时器
	private float m_Errortime = 0.0f;

	//生成计时器ActiveTimer
	private float m_ActiveTimer = 0.0f;

	const float ACTIVE_TIME = 1.0f;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        
        //从我的父亲获取Body_Manager
        m_Body_Manager = transform.parent.gameObject;
        //从Body_Mannager获取bodyParts
        var m_bodyParts = m_Body_Manager.GetComponent<Body_Manager>().bodyParts;
        var entry = m_bodyParts.FirstOrDefault(x => x.Value == gameObject);
        if (entry.Value != null)
        {
	        m_bodynumber = entry.Key;
        }
        
        // Debug.Log("Body number: " + m_bodynumber);
        
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
    
    public void GenerateError(string layer)
    {
	    //如果layer是Flesh，那么我就生成一个在m_fleshlayer上的Error
	    if (layer == "Flesh")
	    {
		    m_error_Flesh = Instantiate(m_errorPrefab, m_fleshlayer.transform);
		    m_error_Flesh.transform.position = m_fleshlayer.transform.position;
	    }
	    else if (layer == "Machine")
	    {
		    m_error_Machine = Instantiate(m_errorPrefab, m_mechaniclayer.transform);
		    m_error_Machine.transform.position = m_mechaniclayer.transform.position;
	    }
    }
    
    
    public void OnBodyStateActive()
    {
		ChangeAlpha(0.5f);
		//switch一下所在哪个层级，如果在FleshLayer，那么m_fleshlayer_Logic.isActivated = true;否则m_mechaniclayer_Logic.isActivated = tr
		switch (m_layer)
		{
			case Layer.FLESH:
				m_fleshlayer_Logic.isActivated = true;
				m_mechaniclayer_Logic.isActivated = false;
				m_nervelayer_Logic.isActivated = false;
				break;
			case Layer.MACHINE:
				m_mechaniclayer_Logic.isActivated = true;
				m_nervelayer_Logic.isActivated = false;
				m_fleshlayer_Logic.isActivated = false;
				break;
			case Layer.NERVE:
				m_nervelayer_Logic.isActivated = true;
				m_fleshlayer_Logic.isActivated = false;
				m_mechaniclayer_Logic.isActivated = false;
				break;
		}

		string bodynumber = m_bodynumber;
		GameEventArgs args = new GameEventArgs{StringValue = bodynumber};
		//发布事件，用一个string传递
		EventManager.Instance.TriggerEvent("BodyActiveReport",args);
		
    }
    
    public void OnBodyStateInactive()
    {
		//透明度改为0.6
		ChangeAlpha(0.1f);
		
		m_nervelayer_Logic.isActivated = false;
		m_mechaniclayer_Logic.isActivated = false;
		m_fleshlayer_Logic.isActivated = false;

    }

	public void OnIsRepairing()
	{
		ChangeAlpha(0.1f);
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

        }
        
        if (m_isRepairing)
        {
	        OnIsRepairing();
        }
    }
    
    public void UpdatehasError()
    {
	    //如果有m_error_Flesh或者m_error_Machine，那么我就有Error,我对应的hasError_Flesh或者hasError_Machine也为true
	    if (m_error_Flesh != null)
	    {
		    hasError_Flesh = true;
	    }
	    else
	    {
		    hasError_Flesh = false;
	    }
	    
	    if (m_error_Machine != null)
	    {
		    hasError_Machine = true;
	    }
	    else
	    {
		    hasError_Machine = false;
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
	    //查看当前我所在的层，如果我在FleshLayer，那么我就摧毁FleshLayer上的Error；如果我在MachineLayer，那么我就摧毁MachineLayer上的Error
	    if (Layer_Handler.Instance.m_layer == Layer.FLESH)
	    {
		    m_error_Flesh.GetComponent<ErrorLogic>().DestroyError();
		    return;
	    }
	    else if (Layer_Handler.Instance.m_layer == Layer.MACHINE)
	    {
		    m_error_Machine.GetComponent<ErrorLogic>().DestroyError();
		    return;
	    }

            Debug.Log("No error to destroy");
    }
    

	//判断，如果有Error且ActiveTimer大于Destroytime
    public void CheckIntoRepair()
    {
	    //定义一个bool，就是如果现在的Layer_Handler.Instance.m_layer下我有对应的hasError，那么就返回true;否则返回false
   		bool isCanRepair = UpdateIsCanRepair();	    	    
	    
	    
	    m_repairingSubMode = ControlMode_Manager.Instance.m_repairingSubMode;
	    //针对不同的RepairingSubMode，进行不同的操作
	    switch (m_repairingSubMode)
	    {
		    case RepairingSubMode.ERROR_REPAIR:
			    if (isCanRepair && Input.GetButton("Fire1") && m_bodyState == BodyState.Active)
			    {
				    SoundManager.Instance.PlaySFX(2);
				    //进入Repair状态
				    ControlMode_Manager.Instance.ChangeControlMode(ControlMode.REPAIRING);
				    //我自己也进入Repair状态
				    m_isRepairing = true;
			    }
			    break;
		    
		    case RepairingSubMode.CLOCKWORK_REPAIR:
			    break;
	    }
    }

	public bool UpdateIsCanRepair()
	{
	    //定义一个bool，就是如果现在的Layer_Handler.Instance.m_layer下我有对应的hasError，那么就返回true;否则返回false
    	bool isCanRepair = false;

    	if (GenerationStage_Handler.Instance.m_isDoubleError)
    	{
        	bool isDoubleErrorMachineUncleared = m_Body_Manager.GetComponent<Body_Manager>().hasError_Machine;
			Debug.Log("isDoubleErrorMachineUncleared: " + isDoubleErrorMachineUncleared);
        	if (isDoubleErrorMachineUncleared && Layer_Handler.Instance.m_layer == Layer.FLESH)
        	{
            	isCanRepair = false;
        	}
        	else
        	{
            	if (Layer_Handler.Instance.m_layer == Layer.FLESH)
            	{
                	if (hasError_Flesh)
                	{
                    	isCanRepair = true;
                	}
            	}
            	if (Layer_Handler.Instance.m_layer == Layer.MACHINE)
            	{
                	Debug.Log("hasError_Machine: " + hasError_Machine);
                	if (hasError_Machine)
                	{
                    	isCanRepair = true;
                	}
            	}
        	}
    	}
    	else
    	{
        	if (Layer_Handler.Instance.m_layer == Layer.FLESH)
        	{
            	if (hasError_Flesh)
            	{
                	isCanRepair = true;
            	}
        	}
        	if (Layer_Handler.Instance.m_layer == Layer.MACHINE)
        	{
            	Debug.Log("hasError_Machine: " + hasError_Machine);
            	if (hasError_Machine)
            	{
                	isCanRepair = true;
            	}
        	}
    	}

    	return isCanRepair;
	}

    
    public void CheckOutofRepair()
	{
		//如果我自己isrepairing == true，但是Mode是NAVIGATION，那么我就进入isrepairing == false
		if (m_isRepairing == true && ControlMode_Manager.Instance.m_controlMode == ControlMode.NAVIGATION)
		{
			SoundManager.Instance.PlaySFX(2);
			m_isRepairing = false;
		}
	}

	//当有东西被修复时
	public void OnSomethingRepaired(GameEventArgs args)
    {
		//m_isRepairing == true，那么我就进入Inactive状态，并且我摧毁我的Error
		if (m_isRepairing)
		{
		    switch (m_repairingSubMode)
		    {
			    case RepairingSubMode.ERROR_REPAIR:
				    DestroyError();
				    m_bodyState = BodyState.Inactive;
				    m_isRepairing = false;
				    //颜色改为白色
				    ChangeColor(Color.white);
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
				break;
		}
	}
	
	
	void EnableRenderers(Renderer[] renderers, bool enabled)
	{
		foreach (var renderer in renderers)
		{
			renderer.enabled = enabled;
			if (renderer is SpriteRenderer spriteRenderer)
			{
				// spriteRenderer.sortingOrder = 10;
			}
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
		ControlMode_Manager.Instance.m_repairingSubMode = RepairingSubMode.ERROR_REPAIR;
	}
	
	private void OnNerveLayer()
	{
		
		//改变m_nervelayer的isError数值
		m_nervelayer_Logic.hasError_Flesh = hasError_Flesh;
		m_nervelayer_Logic.hasError_Machine = hasError_Machine;

	}

	public void UpdateLayerDisplay()
	{
		m_layer = Layer_Handler.Instance.m_layer;

		// 获取m_fleshlayer, m_mechaniclayer, m_nervelayer及其子物体的所有Renderer
		Renderer[] fleshlayerRenderers = m_fleshlayer.GetComponentsInChildren<Renderer>();
		Renderer[] mechaniclayerRenderers = m_mechaniclayer.GetComponentsInChildren<Renderer>();
		Renderer[] nervelayerRenderers = m_nervelayer.GetComponentsInChildren<Renderer>();

		
		// //如果canRender为false，那么关闭所有的Renderer
		// if (!m_canRender)
		// {
		// 	EnableRenderers(fleshlayerRenderers, false);
		// 	EnableRenderers(mechaniclayerRenderers, false);
		// 	EnableRenderers(nervelayerRenderers, false);
		// 	return;
		// }
		
		m_spriteRenderer.enabled = true;

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
		
		//当然，也要根据m_canRenderFlesh, m_canRenderMachine, m_canRenderNerve来决定是否显示，如果这个是false，那么就关闭对应的Renderer
		if (!m_canRender_Flesh)
		{
			EnableRenderers(fleshlayerRenderers, false);
		}

		if (!m_canRender_Machine)
		{
			EnableRenderers(mechaniclayerRenderers, false);
		}
		if(!m_canRender_Nerve)
		{
			EnableRenderers(nervelayerRenderers, false);
		}

	}
}
