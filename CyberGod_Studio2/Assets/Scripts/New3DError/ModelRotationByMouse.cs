using UnityEngine;

public class ModelRotationByMouse : MonoBehaviour
{
 
    [SerializeField]
    private float sensitivity = 2f;
    

    void Update()
    {
        if (true||Add3DErrorByCube.ifAddDone)
        {
            float rotation = Input.GetAxis("Mouse X") * sensitivity;

            // ���m_game_manager.Instance.isPauseΪ�棬����
            /*
            if (m_GameManager.Instance.isPaused)
            {
                return;
            }
             */
            transform.Rotate(new Vector3(0, rotation, 0));
           
        }
        
    }
}