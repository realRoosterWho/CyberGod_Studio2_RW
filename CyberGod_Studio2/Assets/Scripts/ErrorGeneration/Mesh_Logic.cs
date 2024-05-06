using UnityEngine;

public class Mesh_Logic : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 1f;

    void Update()
    {
        float rotation = Input.GetAxis("Mouse X") * sensitivity;
        
        //如果m_game_manager.Instance.isPause为真，返回
        if (m_GameManager.Instance.isPaused)
        {
            return;
        }
        transform.Rotate(new Vector3(0, rotation, 0));
    }
}