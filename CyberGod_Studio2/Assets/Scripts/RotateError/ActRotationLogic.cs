using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActRotationLogic : MonoBehaviour
{
    public Quaternion targetRotation; // 目标旋转
    public Quaternion currentRotation; // 当前旋转
    
    [Header("Rotation Settings")]
    [SerializeField] private float rotationSpeed = 5.0f; // 旋转速度
    [SerializeField] private float m_maxRotationSpeed = 10.0f; // 最大旋转速度

    // Start is called before the first frame update
    void Start()
    {
        // 获取并存储当前的旋转作为目标旋转
        targetRotation = transform.rotation;
        // 初始化当前旋转，使其在Z轴上与目标旋转相差一个随机角度
        int deadzone = 45;
        int randomRotation = Random.Range(deadzone, 360 - deadzone);
        currentRotation = targetRotation * Quaternion.Euler(0, 0, randomRotation);

        // 将当前旋转赋值给Transform的旋转
        transform.rotation = currentRotation;
    }

    // Update is called once per frame
    void Update()
    {
        // 根据鼠标的移动更新当前旋转
        UpdateCurrentRotation();

        // 检查当前旋转和目标旋转的对齐情况
        CheckAlignment();
    }

    // 更新当前旋转的函数
    private void UpdateCurrentRotation()
    {
        float maxRotationSpeed = m_maxRotationSpeed; // 您可以根据需要调整这个值
        float mouseInput = Input.GetAxis("Mouse X"); // 获取鼠标的水平移动

        // 计算旋转量并限制最大旋转速度
        float rotation = mouseInput * rotationSpeed;
        rotation = Mathf.Clamp(rotation, -maxRotationSpeed, maxRotationSpeed);

        // 计算目标旋转
        Quaternion targetRotation = currentRotation * Quaternion.Euler(new Vector3(0, 0, rotation));

        // 使用Lerp函数平滑过渡到目标旋转
        currentRotation = Quaternion.Lerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = currentRotation;
    }

    // 检查对齐情况的函数
    private void CheckAlignment()
    {
        // 如果当前旋转和目标旋转的差距小于10度，并且按下了"Fire1"按钮
        if (Quaternion.Angle(currentRotation, targetRotation) < 10 && Input.GetButtonDown("Fire1"))
        {
            
            Debug.Log("Self Repaired");
            // 触发OnSelfRepaired函数
            OnSelfRepaired();
        }
    }

    // 当自身修复时触发的函数
    private void OnSelfRepaired()
    {
        // 触发SomethingRepaired事件
        EventManager.Instance.TriggerEvent("SomethingRepaired", new GameEventArgs());

        // 摧毁父物体
        Destroy(transform.parent.gameObject);
    }
}