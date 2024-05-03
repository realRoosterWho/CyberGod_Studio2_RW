using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;


public class Body_Manager : SerializedMonoBehaviour
{
    [SerializeField] public Dictionary<string, GameObject> bodyParts = new Dictionary<string, GameObject>();
    private Dictionary<string, BodyPos_Logic> bodyPartLogics = new Dictionary<string, BodyPos_Logic>();
	private BodyPos_Logic bodylogic;
	private GameEventArgs m_args;
    
    //定义一个列表，用于存储有错误的身体部位
    [SerializeField] public List<string> errorBodyParts = new List<string>();
    
    //定义一个列表，用于存储可以生成错误的身体部位
    [SerializeField] public List<string> errorGeneratableBodyParts = new List<string>();
    
    [SerializeField]public SpriteRenderer m_nerveLayerRenderer;



    void Start()
    {
        EventManager.Instance.AddEvent("MotionCaptureInput", OnMotionCaptureInput);
		//初始化Body_Logic Dictionary
        foreach (var bodyPart in bodyParts)
        {
            var bodyLogic = bodyPart.Value.GetComponent<BodyPos_Logic>();
            if (bodyLogic != null)
            {
                bodyPartLogics.Add(bodyPart.Key, bodyLogic);
            }
        }
        
        //TEST
        // GenerateRandomError();
    }

	void Update()
    {

        UpdateLayerFunction();
        
		//如果ControllerMode是NAVIGATION，那么就调用HandleMotionCaptureInput函数
		if (ControlMode_Manager.Instance.m_controlMode == ControlMode.NAVIGATION)
        {
            HandleMotionCaptureInput(m_args);
        }

        UpdateErrorBodyParts();
        UpdateErrorGeneratableBodyParts();

        //HandleMotionCaptureInput(m_args);
    }

    void OnMotionCaptureInput(GameEventArgs args)
    {
		m_args = args;
    }

	void HandleMotionCaptureInput(GameEventArgs args)
    {
		//如果没有传入参数，直接返回
		if (args == null)
		{
		    return;
		}

    	MainThreadDispatcher.ExecuteInUpdate(() =>
		{
            string arg_key = args.FloatValue.ToString();

            if (bodyPartLogics.ContainsKey(arg_key))
            {
                var bodyLogic = bodyPartLogics[arg_key];//获取这个组件
                if (bodyLogic != null)
                {
                    bodyLogic.BodyStateChange("Active");
                }
            }

            foreach (var bodyPartLogic in bodyPartLogics)
            {
                if (bodyPartLogic.Key == arg_key)
                {
                    continue;
                }

                if (bodyPartLogic.Value != null)
                {
					var bodyLogic = bodyPartLogic.Value; //var的意思是自动识别类型
					bodyLogic.BodyStateChange("Inactive");
                }
            }
        });
    }
	//在这个字典中随机抽取一个值，并且调用一个Body_Logic的GenerateError函数
    public void GenerateRandomError()
    {
        bool errorGenerated = false;
        int counter = 0;

        // 循环直到生成一个错误或遍历完所有可生成错误的身体部位
        while (!errorGenerated)
        {
            var randomIndex = Random.Range(0, errorGeneratableBodyParts.Count);
            var randomBodyPartKey = errorGeneratableBodyParts[randomIndex];

            if (bodyPartLogics.ContainsKey(randomBodyPartKey))
            {
                var randomBodyPart = bodyPartLogics[randomBodyPartKey];

                if (!randomBodyPart.hasError)
                {
                    randomBodyPart.GenerateError();
                    // Debug.Log("Generated error in " + randomBodyPartKey);
                    errorGenerated = true; // 设置标志位表示已生成错误
                }
            }

            counter++;

            //如果已经遍历完所有可生成错误的身体部位，或者循环次数超过了errorGeneratableBodyParts的数量，退出循环
            if (errorGeneratableBodyParts.All(bodyPartKey => bodyPartLogics[bodyPartKey].hasError) || counter >= errorGeneratableBodyParts.Count)
            {
                Debug.Log("All generatable body parts have errors or looped through all generatable body parts");
                break;
            }
        }
    }
    
    public void GenerateError(string bodyPart)
    {
        if (bodyPartLogics.ContainsKey(bodyPart))
        {
            var bodyLogic = bodyPartLogics[bodyPart];
            //检查是否已经有错误
            if (!bodyLogic.hasError)
            {
                bodyLogic.GenerateError();
            }
        }
    }
    
    private void UpdateErrorBodyParts()
    {
        errorBodyParts.Clear();
        foreach (var bodyPartLogic in bodyPartLogics)
        {
            if (bodyPartLogic.Value.hasError)
            {
                errorBodyParts.Add(bodyPartLogic.Key);
            }
        }
    }
    
    public int GetErrorNumber()
    {
        int errorNumber = errorBodyParts.Count;
        return errorNumber;
    }
    
    //定义一个函数，用于在对应的层级执行对应的操作
    public void UpdateLayerFunction()
    {
        switch (Layer_Handler.Instance.m_layer)
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
    
    public void UpdateErrorGeneratableBodyParts()
    {
        //如果不在列表里，把这个Gameobject直接禁用
        foreach (var bodyPart in bodyParts)
        {
            if (!errorGeneratableBodyParts.Contains(bodyPart.Key))
            {
                //把bodypart的bodypos_logic的canRender设置为false
                bodyPart.Value.GetComponent<BodyPos_Logic>().m_canRender = false;
            }
        }
    }
    
    private void OnFleshLayer()
    {
        m_nerveLayerRenderer.enabled = false;
    }
    
    private void OnMachineLayer()
    {
        m_nerveLayerRenderer.enabled = false;

    }
    
    private void OnNerveLayer()
    {
        m_nerveLayerRenderer.enabled = true;
    }

}