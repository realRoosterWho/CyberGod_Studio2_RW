#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations; // 使用UnityEngine.Animations，而不是UnityEditor.Animations
using System;
using Unity.VisualScripting;

public class Connect : MonoBehaviour
{
    private void Update()
    {
        if (U2P.Instance.isConnected)
        {
            float[] data = U2P.Instance.RecData();
            if (data != null)
            {
                print(data.Length);
                print("Received data: ");
                U2P.Instance.SendData(new List<float>() { 0 });
                // 这是一个示例，将接收到的数据打印出来
                // 你可以在这里编写你的逻辑代码
                for (int i = 0; i < data.Length; i++)
                {
                    print(data[i]);
                }
            }
        }
    }
}
