using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class layerUI_test_Logic : MonoBehaviour
{
    TextMeshProUGUI m_textMeshProUGUI; // Correct class name is TextMeshProUGUI

    // Start is called before the first frame update
    void Start()
    {
        m_textMeshProUGUI = GetComponent<TextMeshProUGUI>(); // Correct class name is TextMeshProUGUI
    }

    // Update is called once per frame
    void Update()
    {
        m_textMeshProUGUI.text = "Layer: " + Layer_Handler.Instance.m_layer.ToString();
    }
}