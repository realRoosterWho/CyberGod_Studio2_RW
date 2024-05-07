#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations; // 使用UnityEngine.Animations，而不是UnityEditor.Animations
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
                print("���յ�����");
                U2P.Instance.SendData(new List<float>() { 0 });
                print(data[0]);
            }
        }
    }
}
