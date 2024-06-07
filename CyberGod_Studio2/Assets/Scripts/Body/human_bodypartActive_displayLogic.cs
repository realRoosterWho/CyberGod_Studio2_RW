using UnityEngine;

public class human_bodypartActive_displayLogic : MonoBehaviour
{
    private SpriteRenderer m_spriteRenderer;
    private BodyPos_Logic m_bodyPos_Logic;
    private Layer_Handler m_layerHandler;

    public float lerpSpeed = 1f; // 控制Lerp的速度
    public float m_targetAlpha = 0.3f; // 目标Alpha值
    private float targetAlpha; // 目标Alpha值

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_bodyPos_Logic = transform.parent.GetComponent<BodyPos_Logic>();
        m_layerHandler = Layer_Handler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_layerHandler.m_layer == Layer.MACHINE || m_layerHandler.m_layer == Layer.FLESH)
        {
            switch (m_bodyPos_Logic.m_bodyState)
            {
                case BodyPos_Logic.BodyState.Inactive:
                    targetAlpha = 0f;
                    break;
                case BodyPos_Logic.BodyState.Active:
                    targetAlpha = m_targetAlpha;
                    break;
            }
        }
        else
        {
            targetAlpha = 0f;
        }

        UpdateAlpha();
    }

    void UpdateAlpha()
    {
        float currentAlpha = m_spriteRenderer.color.a;
        float newAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * lerpSpeed);
        m_spriteRenderer.color = new Color(m_spriteRenderer.color.r, m_spriteRenderer.color.g, m_spriteRenderer.color.b, newAlpha);
    }
}