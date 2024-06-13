using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReadyReader : MonosingletonTemp<CameraReadyReader>
{
    public float data; // 公共变量用于存储数据

    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.AddEvent("MotionCaptureInput", OnMotionCaptureInput);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(data);
    }

    void OnMotionCaptureInput(GameEventArgs args)
    {
        data = args.FloatValue;
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEvent("MotionCaptureInput", OnMotionCaptureInput);
    }
}