using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_CameraReady : MonoBehaviour
{
    public main_bg_Logic m_main_bg_Logic;
    // Get TextMeshProUGUI component
    public TextMeshProUGUI textMeshProUGUI;
    public TextMeshProUGUI textMeshProUGUI2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Monitor CameraReadyReader's data
        if (CameraReadyReader.Instance.data == 0)
        {
            textMeshProUGUI.text = "You need to use launcher to launch the program. Please exit.";
        }
        else if (CameraReadyReader.Instance.data == 99)
        {
            textMeshProUGUI.text = "Camera started. Place your arm in front of your chest to check if the distance is appropriate.";
        }
        else
        {
            textMeshProUGUI.text = "Distance appropriate";
            m_main_bg_Logic.canJump = true;
            textMeshProUGUI2.enabled = true;
        }
    }
}
