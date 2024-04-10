using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;
using System;
using Unity.VisualScripting;

public class Communication : MonoBehaviour
{
    private void Update()
    {
        if (U2P.Instance.isConnected)
        {
            float[] data = U2P.Instance.RecData();
            if (data != null)
            {
                print("接收到数据");
                U2P.Instance.SendData(new List<float>() { 0 });
                print(data[0]);
            }
        }
    }
}
