using System.Collections.Generic;
using UnityEngine;

public class nerve_bodypartActive_displayLogic : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private BodyPos_Logic m_bodyPos_Logic;
    private Layer_Handler m_layerHandler;
    private Body_Manager m_bodyManager;
    private Material m_material; // 新增材质变量
    
    //新建particlesystem变量
    private NewMeshErrorParticleSystem m_NewMeshErrorParticleSystem;
    private bool isparticleSystem = false;
    private bool hasError = false;
    
    [SerializeField] private bool isLine = false;
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
        //获取m_NewMeshErrorParticleSystem组件在子物体上
        m_NewMeshErrorParticleSystem = GetComponentInChildren<NewMeshErrorParticleSystem>();
        //如果粒子系统不为空，则将isparticleSystem设置为true
        if (m_NewMeshErrorParticleSystem != null)
        {
            isparticleSystem = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //获取m_bodyManager的errorGeneratableBodyParts_Nerve列表，判断当前的bodynumber是否在列表中
        if (m_bodyManager.errorBodyParts_Flesh.Contains(m_bodyPos_Logic.m_bodynumber) || m_bodyManager.errorBodyParts_Machine.Contains(m_bodyPos_Logic.m_bodynumber))
        {
            Debug.Log("This body part has errorNerveBodypartActive_displayLogic");
            hasError = true;
        }
        else
        {
            hasError = false;
        }
        
        if (m_layerHandler.m_layer == Layer.NERVE)
        {
            switch (m_bodyPos_Logic.m_bodyState)
            {
                case BodyPos_Logic.BodyState.Inactive:
                    ChangeMaterialProperties(m_material, 0.01f, 0.01f, 0.01f, 0.01f, Color.white); // 在Inactive状态下去除描边
                    //如果isparticleSystem为true，则停止播放粒子系统
                    if (isparticleSystem)
                    {
                        m_NewMeshErrorParticleSystem.StopParticleSystem();
                    }
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
                    //如果有错误，则ParticleHas，否则ParticleHasNot
                    if (hasError)
                    {
                        if (isparticleSystem)
                        {
                            ParticleHas();
                        }
                    }
                    else
                    {
                        if (m_bodyManager.errorGeneratableBodyParts_Nerve.Contains(m_bodyPos_Logic.m_bodynumber))
                        {
                            if (isparticleSystem)
                            {
                                ParticleHasNot();
                            }
                        }
                        else
                        {
                            //如果isparticleSystem为true，则停止播放粒子系统
                            if (isparticleSystem)
                            {
                                m_NewMeshErrorParticleSystem.StopParticleSystem();
                            }
                            if (isLine)
                            {
                                m_spriteRenderer.sprite = m_sprites[0];
                            }
                            else
                            {
                                m_spriteRenderer.sprite = m_sprites[1];
                            }
                        }

                    }
                    break;
            }
            m_spriteRenderer.enabled = true;
        }
        else
        {
            //如果isparticleSystem为true，则停止播放粒子系统
            if (isparticleSystem)
            {
                m_NewMeshErrorParticleSystem.StopParticleSystem();
            }
            m_spriteRenderer.enabled = false;
        }
    }



    public void ParticleHas()
    {
        Debug.Log("ParticleHas");
        //如果有错误，则播放粒子系统，并且SetStartLifetime(float lifetime)设置为0.4f
        m_NewMeshErrorParticleSystem.PlayParticleSystem();
        m_NewMeshErrorParticleSystem.SetDuration(0.4f);
    }
    
    public void ParticleHasNot()
    {
        //如果没有错误，则播放粒子系统，并且SetStartLifetime(float lifetime)设置为1.6f
        m_NewMeshErrorParticleSystem.PlayParticleSystem();
        m_NewMeshErrorParticleSystem.SetDuration(1.6f);
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