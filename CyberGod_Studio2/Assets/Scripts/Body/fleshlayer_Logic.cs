using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class fleshlayer_Logic : MonoBehaviour
{
	
	[SerializeField]public int m_initialSortingOrder = 3;
    [SerializeField]public bool isActivated = false;
    public ObjectInfo info;
	private ObjectInfo info_temp;

	//一个材质列表
	private List<Material> m_materials = new List<Material>();

	//一个SpriteRenderer列表
	private List<SpriteRenderer> m_spriteRenderers = new List<SpriteRenderer>();

	// 新增的成员变量
    private BodyPos_Logic m_bodyPos_Logic;
    private Body_Manager m_bodyManager;

    // Start is called before the first frame update
    void Start()
    {
		//获取所有子物体的材质，加入到材质列表中，用GetComponentsInChildren
		m_materials = GetComponentsInChildren<Renderer>().Select(r => r.material).ToList();

		//获取所有子物体的SpriteRenderer，加入到SpriteRenderer列表中，用GetComponentsInChildren
		m_spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();

		
        // 获取父物体上的BodyPos_Logic组件
        m_bodyPos_Logic = transform.parent.GetComponent<BodyPos_Logic>();
        // 获取Body_Manager组件
        m_bodyManager = transform.parent.parent.GetComponent<Body_Manager>();

		


    }

	void Update()
	{
		// 检查m_bodyPos_Logic.m_bodynumber是否在m_bodyManager.errorGeneratableBodyParts_Flesh列表中
        if (!m_bodyManager.errorGeneratableBodyParts_Flesh.Contains(m_bodyPos_Logic.m_bodynumber))
        {
            info_temp = new ObjectInfo {name = "无义体", description = "未查询到此部位义体"};
        }
		else
		{
			if (info.name == "")
   			{
        		info_temp = new ObjectInfo {name = "无义体", description = "未查询到此部位义体"};
    		}
			else
			{
				info_temp = info;
			}
		}


    	if (isActivated && ControlMode_Manager.Instance.m_controlMode != ControlMode.REPAIRING)
    	{
        	UIDisplayManager.Instance.DisplayLeftInfo(info_temp);
        	DialogueManager.Instance.RequestSpiritSpeakEntry("flesh");
    	}




    	// 如果isActivated为true，就调用ChangeMaterialProperties函数
    	if (isActivated)
    	{
    	    ChangeMaterialProperties(6f, 1f, 1f, 1f, Color.white);
			ChangeSpriteRendererSortingOrder(10);
    	}
    	else
    	{
        	ChangeMaterialProperties(0.01f, 0.01f, 0.01f, 0.01f, Color.white);
			ChangeSpriteRendererSortingOrder(m_initialSortingOrder);

    	}
	}


	public void ChangeMaterialProperties(float ltSoft, float ltExpand, float ltOuterStrength, float ltInnerStrength, Color ltColor)
	{
    	// 遍历所有的材质
    	foreach (var material in m_materials)
    	{
        	// 更改每个材质的参数
        	material.SetFloat("_LtSoft", ltSoft);
        	material.SetFloat("_LtExpand", ltExpand);
        	material.SetFloat("_LtOuterStrength", ltOuterStrength);
        	material.SetFloat("_LtInnerStrength", ltInnerStrength);
        	material.SetColor("_LtColor", ltColor);
    	}
	}

	//定义一个函数，用于改变SpriteRenderer的SortingOrder
	public void ChangeSpriteRendererSortingOrder(int sortingOrder)
    {
    	//遍历所有的SpriteRenderer
    	foreach (var spriteRenderer in m_spriteRenderers)
    	{
        	//设置每个SpriteRenderer的SortingOrder
        	spriteRenderer.sortingOrder = sortingOrder;
    	}
    }
    
    
}
