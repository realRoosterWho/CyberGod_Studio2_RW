using System.Collections.Generic;
using UnityEngine;

public class nerve_bodypartActive_displayLogic : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private BodyPos_Logic m_bodyPos_Logic;
    private Layer_Handler m_layerHandler;
    private Body_Manager m_bodyManager;
    private Material m_material; // 新增材质变量

    [SerializeField]
    private List<Sprite> m_sprites;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_bodyPos_Logic = transform.parent.GetComponent<BodyPos_Logic>();
        m_layerHandler = Layer_Handler.Instance;
        m_bodyManager = transform.parent.parent.GetComponent<Body_Manager>();
        m_material = GetComponent<Renderer>().material; // 获取材质
    }

    // Update is called once per frame
    void Update()
    {
        if (m_layerHandler.m_layer == Layer.NERVE)
        {
            switch (m_bodyPos_Logic.m_bodyState)
            {
                case BodyPos_Logic.BodyState.Inactive:
                    ChangeMaterialProperties(m_material, 0.01f, 0.01f, 0.01f, 0.01f, Color.white); // 在Inactive状态下去除描边
                    if (m_bodyManager.errorGeneratableBodyParts_Nerve.Contains(m_bodyPos_Logic.m_bodynumber))
                    {
                        m_spriteRenderer.sprite = m_sprites[2];
                    }
                    else
                    {
                        m_spriteRenderer.sprite = m_sprites[0];
                    }
                    break;
                case BodyPos_Logic.BodyState.Active:
                    m_spriteRenderer.sprite = m_sprites[1];
                    ChangeMaterialProperties(m_material, 6f, 1f, 1f, 1f, Color.white); // 在Active状态下添加描边
                    break;
            }
            m_spriteRenderer.enabled = true;
        }
        else
        {
            m_spriteRenderer.enabled = false;
        }
    }

    // 定义一个函数，用于改变材质属性
    public void ChangeMaterialProperties(Material material, float ltSoft, float ltExpand, float ltOuterStrength, float ltInnerStrength, Color ltColor)
    {
        // Change the parameters of the material
        material.SetFloat("_LtSoft", ltSoft);
        material.SetFloat("_LtExpand", ltExpand);
        material.SetFloat("_LtOuterStrength", ltOuterStrength);
        material.SetFloat("_LtInnerStrength", ltInnerStrength);
        material.SetColor("_LtColor", ltColor);
    }
}