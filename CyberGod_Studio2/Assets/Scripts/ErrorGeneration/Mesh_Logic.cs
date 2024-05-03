using UnityEngine;

public class Mesh_Logic : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 1f;

    void Update()
    {
        float rotation = Input.GetAxis("Mouse X") * sensitivity;
        transform.Rotate(new Vector3(0, rotation, 0));
    }
}