using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyCubeProceesError : MonoBehaviour
{

    public List<GameObject> collidedObjects = new List<GameObject>();
    private bool isTriggered = false;
    private bool isClicked = false;
    private bool eventTriggered = false;
    private Material m_material;
    private void Start()
    {
        //��ȡ�Լ��Ĳ���
        m_material = GetComponent<Renderer>().material;
        //���Լ����ʵ�͸���Ƚ��ͣ��ҵĲ����õ���Sprite-Default
        Color color = m_material.color;
        color.a = 0.05f;
        m_material.color = color;

    }
    // Start is called before the first frame update
    void Update()
    {
        isClicked = false;
        eventTriggered = false;
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }

        UpdateColliderCheck();
    }

    void UpdateColliderCheck()
    {
        foreach (var obj in collidedObjects)
        {
            if (isClicked)
            {
                StartCoroutine(DestroyAfterDelay(obj, 0.25f));
            }
            else if (!eventTriggered)
            {
                EventManager.Instance.TriggerEvent("CanRepairSomething", new GameEventArgs());
                eventTriggered = true;
            }
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("3DError"))
        {
            if (!collidedObjects.Contains(other.gameObject))
            {
                collidedObjects.Add(other.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("3DError"))
        {
            collidedObjects.Remove(other.gameObject);
        }
    }
    
    
    
    // void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("3DError"))
    //     {
    //         Debug.Log("triggered");
    //         if (!collidedObjects.Contains(other.gameObject))
    //         {
    //             collidedObjects.Add(other.gameObject);
    //         }
    //     }
    //     else
    //     {
    //         Debug.Log("nottriggered");
    //     }
    // }

    // void OnTriggerStay(Collider other)
    // {
    //     if (other.gameObject.CompareTag("3DError") && isClicked)
    //     {
    //         StartCoroutine(DestroyAfterDelay(other.gameObject, 0.25f));
    //     }
    //     
    //     if (other.gameObject.CompareTag("3DError"))
    //     {
    //         EventManager.Instance.TriggerEvent("CanRepairSomething", new GameEventArgs());
    //     }
    // }

    // IEnumerator DestroyAfterDelay(GameObject gameObject, float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     EventManager.Instance.TriggerEvent("ErrorDestroyed", new GameEventArgs());
    //     Destroy(gameObject);
    //     Debug.Log("repairederror");
    //     SoundManager.Instance.PlaySFX(1);
    //     isClicked = false;
    // }
    
    
    IEnumerator DestroyAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.Instance.TriggerEvent("ErrorDestroyed", new GameEventArgs());
        collidedObjects.Remove(gameObject);
        Destroy(gameObject);
        isClicked = false;
    }
}
