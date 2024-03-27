using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTest : MonoBehaviour
{
    public List<Vector2> mousePositions = new List<Vector2>();
    private float distanceX = 0;
    private float distanceY = 0;

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePositions.Add(mousePosition);
        
        //检测鼠标累计位移量，分别展示
        if (mousePositions.Count > 1)
        {
            distanceX += Mathf.Abs(mousePositions[mousePositions.Count - 1].x - mousePositions[mousePositions.Count - 2].x);
            distanceY += Mathf.Abs(mousePositions[mousePositions.Count - 1].y - mousePositions[mousePositions.Count - 2].y);
            Debug.Log("distanceX: " + distanceX + " distanceY: " + distanceY);
        }

        
        //单击左键清零distance
        if (Input.GetMouseButtonDown(0))
        {
            distanceX = 0;
            distanceY = 0;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Vector2 mousePosition in mousePositions)
        {
            Gizmos.DrawSphere(mousePosition, 0.1f);
        }
    }
    

}
