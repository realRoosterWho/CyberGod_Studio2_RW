using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UICameraShakeController : MonosingletonTemp<UICameraShakeController>
{
    public Canvas uiCanvas; // Change this to reference the whole Canvas
    private Vector3 initialPosition;

    void Start()
    {
        if (uiCanvas == null)
        {
            uiCanvas = GetComponent<Canvas>();
        }

        initialPosition = uiCanvas.transform.localPosition; // Get the initial position of the Canvas
    }

    void Update()
    {
        // Get the CinemachineIndependentImpulseListener component
        var impulseListener = GetComponent<CinemachineIndependentImpulseListener>();

        // Check if the component exists
        if (impulseListener != null)
        {
            // Get the impulse
            bool haveImpulse = CinemachineImpulseManager.Instance.GetImpulseAt(
                transform.position, impulseListener.m_Use2DDistance, impulseListener.m_ChannelMask, 
                out Vector3 impulsePos, out Quaternion impulseRot);

            if (haveImpulse)
            {
                // Apply the impulse to the Canvas
                uiCanvas.transform.localPosition = initialPosition + new Vector3(impulsePos.x, impulsePos.y, 0);
            }
        }
    }
}