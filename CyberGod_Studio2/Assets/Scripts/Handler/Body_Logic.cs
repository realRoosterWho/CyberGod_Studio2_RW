using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body_Logic : MonoBehaviour
{
    public enum BodyState //这是一个枚举类型，用于表示身体的状态
    {
        Active,
        Inactive
    }
    
    public bool hasError = false;

    SpriteRenderer m_spriteRenderer;

    [SerializeField]BodyState m_bodyState = BodyState.Inactive;

    //获取Error预制体
    [SerializeField] private GameObject m_errorPrefab;
    private GameObject m_error = null;

	//生成计时器
	private float m_Errortime = 0.0f;

	//生成计时器ActiveTimer
	private float m_ActiveTimer = 0.0f;

	const float DESTROY_TIME = 1.0f;

    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        UpdateBodyStateBehavior();
        UpdatehasError();
		UpdateErrorTimeCounting();
		UpdateActiveTimer();
		CheckDestroyError();
    }
    public void ChangeColor(Color color)
    {
        m_spriteRenderer.color = color;
    }

    public void BodyStateChange(string state)
    {
        //将字符串转换为枚举类型
        m_bodyState = (BodyState)System.Enum.Parse(typeof(BodyState), state);
    }
    
    public void GenerateError()
    {
        //生成一个Error，使之成为自己的子物体
        m_error = Instantiate(m_errorPrefab, transform.position, Quaternion.identity);
    }
    
    
    public void OnBodyStateActive()
    {
        ChangeColor(Color.red);
    }
    
    public void OnBodyStateInactive()
    {
        ChangeColor(Color.white);
    }

    public void UpdateBodyStateBehavior()
    {
        Debug.Log("UpdateBodyStateBehavior");

        switch (m_bodyState) //根据不同的状态，执行不同的操作
        {
            case BodyState.Active:
                OnBodyStateActive();
                break;
            case BodyState.Inactive:
                OnBodyStateInactive();
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
        m_error.GetComponent<ErrorLogic>().DestroyError();
    }

	//判断，如果有Error且ActivateTimer大于5秒，就销毁Error
    public void CheckDestroyError()
    {
        if (hasError && m_ActiveTimer > DESTROY_TIME)
        {
            DestroyError();
        }
    }
	
}
