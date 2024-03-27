using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using UnityEditor;

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
                print("接收到数据");
                U2P.Instance.SendData(new List<float>() { 0 });
                // 在这里写对于接收到的数据的处理
                // 用打印测试
                for (int i = 0; i < data.Length; i++)
                {
                    print(data[i]);
                }
            }
        }
    }
}
