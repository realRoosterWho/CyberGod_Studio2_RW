using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonosingletonTemp<EnergyManager>
{
    public List<EnergyPointLogic> energyPoints; // 存储EnergyPointLogic的列表
    public int displayCount; // 调整显示前几个EnergyPointLogic
    public int spriteSwitchCount; // 调整前几个显示第二个Sprite而剩下的显示第一个Sprite

    private bool isAllSpriteSwitchFalse = false;

    // Start is called before the first frame update
    void Start()
    {

        spriteSwitchCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AdjustDisplayAndSprite();

        if (isAllSpriteSwitchFalse)
        {
            GenerationStage_Handler.Instance.LoseGame();
        }
    }

    // 调整EnergyPointLogic的显示和Sprite的函数
    public void AdjustDisplayAndSprite()
    {
        for (int i = 0; i < energyPoints.Count; i++)
        {
            if (i < displayCount)
            {
                // 显示EnergyPointLogic
                energyPoints[i].gameObject.SetActive(true);

                // 根据spriteSwitchCount调整Sprite
                energyPoints[i].SwitchSprite(i >= (spriteSwitchCount));
            }
            else
            {
                // 隐藏EnergyPointLogic
                energyPoints[i].gameObject.SetActive(false);
            }
        }

        if (spriteSwitchCount >= displayCount)
        {
            isAllSpriteSwitchFalse = true;
        }
    }
    
    public void UseEnergyPoint()
    {
        if (ControlMode_Manager.Instance.m_controlMode == ControlMode.NAVIGATION && !(Layer_Handler.Instance.m_layer == Layer.NERVE))
        {
            if (spriteSwitchCount < displayCount)
            {
                spriteSwitchCount++;
            }
        }
    }
}