using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class nervelayer_Logic : SerializedMonoBehaviour
{
    //定义一个列表，用于存储所有贴图
    [InfoBox("0 is non-error, 1 is flesh-error, 2 is mechanic-error")]
    [SerializeField] private List<Sprite> m_sprites = new List<Sprite>();
    
    
    [SerializeField]public bool isActivated = false;
    [SerializeField]public bool isError = false;
    
    
    [SerializeField]public bool hasError_Flesh = false;
    [SerializeField]public bool hasError_Machine = false;
    
    public ObjectInfo info;
    public ObjectInfo info_wrong;
    
    private ObjectInfo info_temp;
    private ObjectInfo info_wrong_temp;
    
    // 新增的成员变量
    private BodyPos_Logic m_bodyPos_Logic;
    private Body_Manager m_bodyManager;
    

    
    //定义颜色f18c24
    private Color m_color = new Color(0.95f, 0.55f, 0.14f);
    
    //获取自己的材质
    private Material m_material;
    
    // Start is called before the first frame update
    void Start()
    {
        
        // 获取父物体上的BodyPos_Logic组件
        m_bodyPos_Logic = transform.parent.GetComponent<BodyPos_Logic>();
        // 获取Body_Manager组件
        m_bodyManager = transform.parent.parent.GetComponent<Body_Manager>();

        //获取自己的材质
        m_material = GetComponent<Renderer>().material;
        
    }

    // Update is called once per frame
    void Update()
    {
        //如果任一错误为true，就调用让hasError为true的函数
        if (hasError_Flesh || hasError_Machine)
        {
            isError = true;
        }
        else
        {
            isError = false;
        }
        
        // 检查m_bodyPos_Logic.m_bodynumber是否在m_bodyManager.errorGeneratableBodyParts_Flesh列表中
        if (!m_bodyManager.errorGeneratableBodyParts_Nerve.Contains(m_bodyPos_Logic.m_bodynumber))
        {
            info_temp = new ObjectInfo {name = "无义体", description = "未查询到此部位义体"};
            info_wrong_temp = new ObjectInfo {name = "无义体", description = "未查询到此部位义体"};
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
            
            if (info_wrong.name == "")
            {
                info_wrong_temp = new ObjectInfo {name = "无义体", description = "未查询到此部位义体"};
            }
            else
            {
                info_wrong_temp = info_wrong;
            }

        }


        ChangeSprite();
        
        
        if (isActivated && ControlMode_Manager.Instance.m_controlMode != ControlMode.REPAIRING)
        {
            if (isError)
            {
                UIDisplayManager.Instance.DisplayLeftInfo(info_wrong_temp);
                Debug.Log("wrong");
            }
            else
            {
                UIDisplayManager.Instance.DisplayLeftInfo(info_temp);
            }

            DialogueManager.Instance.RequestSpiritSpeakEntry("nerve");
        }
        
        //如果isActivated为true，就调用ChangeMaterialProperties函数
        if (isActivated)
        {
            // SoundManager.Instance.ControlAudioFadeTransition(true, 4, 0.5f, 0.3f);
            if (isError)
            {
                ChangeMaterialProperties(m_material, 6f, 1f, 1f, 1f, Color.red);
            }
            else
            {
                ChangeMaterialProperties(m_material, 6f, 1f, 1f, 1f, Color.white);
            }
        }
        else
        {
            ChangeMaterialProperties(m_material, 0.01f, 0.01f, 0.01f, 0.01f, Color.white);
        }
        // SoundManager.Instance.ControlAudioFadeTransition(false, 4, 0.5f, 0.3f);
        
    }
    
    //定义一个函数，用于改变贴图
    public void ChangeSprite(int index)
    {
        //如果index小于0或者大于等于m_sprites的数量，就返回
        if (index < 0 || index >= m_sprites.Count)
        {
            return;
        }
        //获取当前的SpriteRenderer组件
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        //设置当前的贴图
        spriteRenderer.sprite = m_sprites[index];
    }
    
    public void ChangeMaterialProperties(Material material, float ltSoft, float ltExpand, float ltOuterStrength, float ltInnerStrength, Color ltColor)
    {
        // Change the parameters of the material
        material.SetFloat("_LtSoft", ltSoft);
        material.SetFloat("_LtExpand", ltExpand);
        material.SetFloat("_LtOuterStrength", ltOuterStrength);
        material.SetFloat("_LtInnerStrength", ltInnerStrength);
        material.SetColor("_LtColor", ltColor);
    }
    
    //按照是否是isFleshError和isMechanicError来改变贴图
    public void ChangeSprite()
    {
        if (hasError_Flesh)
        {
            ChangeSprite(1);
        }
        else if (hasError_Machine)
        {
            ChangeSprite(2);
        }
        else
        {
            ChangeSprite(0);
        }
    }
    
    
}
