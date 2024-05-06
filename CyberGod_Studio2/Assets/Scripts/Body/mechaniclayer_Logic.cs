using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class mechaniclayer_Logic : MonoBehaviour
{
    

    
    
    
    public ObjectInfo info;

    [SerializeField]public bool isActivated = false;
    private List<Material> m_materials = new List<Material>();
    private List<SpriteRenderer> m_spriteRenderers = new List<SpriteRenderer>();


    
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_materials = GetComponentsInChildren<Renderer>().Select(r => r.material).ToList();
        
        //获取所有子物体的SpriteRenderer，加入到SpriteRenderer列表中，用GetComponentsInChildren
        m_spriteRenderers = GetComponentsInChildren<SpriteRenderer>().ToList();

        
        if (info.name == "")
        {
            info = new ObjectInfo {name = "无义体", description = "未查询到此部位义体"};
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated && ControlMode_Manager.Instance.m_controlMode != ControlMode.REPAIRING)
        {
            UIDisplayManager.Instance.DisplayLeftInfo(info);
            DialogueManager.Instance.RequestSpiritSpeakEntry("mechanic");

        }
        
        // 如果isActivated为true，就调用ChangeMaterialProperties函数
        if (isActivated)
        {
            ChangeMaterialProperties(6f, 1f, 1f, 1f, Color.white);
        }
        else
        {
            ChangeMaterialProperties(0.01f, 0.01f, 0.01f, 0.01f, Color.white);
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
