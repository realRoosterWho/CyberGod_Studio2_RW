using UnityEngine;

public class ModelRotationByMouse : MonoBehaviour
{
 
    [SerializeField]
    private float sensitivity = 2f;
    

    void Update()
    {
        if (Add3DErrorByCube.ifAddDone == true)
        {
            float rotation = Input.GetAxis("Mouse X") * sensitivity;

            // ���m_game_manager.Instance.isPauseΪ�棬����
            if (m_GameManager.Instance.isPaused)
            {
                return;
            }
            transform.Rotate(new Vector3(0, rotation, 0));
        }
        
    }
}