using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyCubeProceesError : MonoBehaviour
{

    private bool isClicked = false;
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
    void FixedUpdate()
    {
        isClicked = false;
        if (Input.GetMouseButtonDown(0))
        {
            isClicked = true;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("3DError") && isClicked)
        {
            StartCoroutine(DestroyAfterDelay(other.gameObject, 0.25f));
        }
    }

    IEnumerator DestroyAfterDelay(GameObject gameObject, float delay)
    {
        yield return new WaitForSeconds(delay);
        EventManager.Instance.TriggerEvent("ErrorDestroyed", new GameEventArgs());
        Destroy(gameObject);
        Debug.Log("repairederror");
        SoundManager.Instance.PlaySFX(1);
        isClicked = false;
    }
}
