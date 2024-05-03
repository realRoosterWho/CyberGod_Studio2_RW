using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nervelayer_Logic : MonoBehaviour
{
    //定义一个列表，用于存储所有贴图
    [SerializeField] private List<Sprite> m_sprites = new List<Sprite>();
    
    [SerializeField]public bool isActivated = false;
    [SerializeField]public bool isError = false;
    
    //定义颜色f18c24
    private Color m_color = new Color(0.95f, 0.55f, 0.14f);
    
    //获取自己的材质
    private Material m_material;
    
    // Start is called before the first frame update
    void Start()
    {
        //获取自己的材质
        m_material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //如果isActivated为true，就调用ChangeMaterialProperties函数
        if (isActivated)
        {
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
    
    
}
