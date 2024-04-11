using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;


public class Body_Manager : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> bodyParts = new Dictionary<string, GameObject>();
    private Dictionary<string, Body_Logic> bodyPartLogics = new Dictionary<string, Body_Logic>();
	private Body_Logic bodylogic;
	private GameEventArgs m_args;


    void Start()
    {
        EventManager.Instance.AddEvent("MotionCaptureInput", OnMotionCaptureInput);
		//初始化Body_Logic Dictionary
        foreach (var bodyPart in bodyParts)
        {
            var bodyLogic = bodyPart.Value.GetComponent<Body_Logic>();
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
		//如果ControllerMode是NAVIGATION，那么就调用HandleMotionCaptureInput函数
		if (ControlMode_Manager.Instance.m_controlMode == ControlMode.NAVIGATION)
        {
            HandleMotionCaptureInput(m_args);
        }
		
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

        // 循环直到生成一个错误或遍历完所有身体部位
        while (!errorGenerated)
        {
            var randomIndex = Random.Range(0, bodyPartLogics.Count);
            var randomBodyPart = bodyPartLogics.Values.ElementAt(randomIndex);

            if (!randomBodyPart.hasError)
            {
                randomBodyPart.GenerateError();
                errorGenerated = true; // 设置标志位表示已生成错误
            }

            counter++;

            //如果已经遍历完所有身体部位，或者循环次数超过了bodyPartLogics的数量，退出循环
            if (bodyPartLogics.Values.All(bodyPart => bodyPart.hasError) || counter >= bodyPartLogics.Count)
            {
                Debug.Log("All body parts have errors or looped through all body parts");
                break;
            }
        }
    }

}