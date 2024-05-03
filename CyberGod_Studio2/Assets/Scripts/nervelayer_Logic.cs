using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nervelayer_Logic : MonoBehaviour
{
    //定义一个列表，用于存储所有贴图
    [SerializeField] private List<Sprite> m_sprites = new List<Sprite>();
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
    
}
