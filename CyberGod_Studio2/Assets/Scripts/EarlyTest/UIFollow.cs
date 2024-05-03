using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour
{
    private RectTransform healthBarUI;
    public Transform character;

    void Start()
    {
        healthBarUI = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        if (healthBarUI != null && character != null)
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(character.position);
            healthBarUI.position = screenPosition;
        }
    }
}
