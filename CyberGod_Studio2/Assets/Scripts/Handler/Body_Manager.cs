using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Body_Manager : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<string, GameObject> bodyParts = new Dictionary<string, GameObject>();
    private Dictionary<string, SpriteRenderer> bodyPartRenderers = new Dictionary<string, SpriteRenderer>();

    void Start()
    {
        EventManager.Instance.AddEvent("MotionCaptureInput", OnMotionCaptureInput);
        
        foreach (var bodyPart in bodyParts)
        {
            var spriteRenderer = bodyPart.Value.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                bodyPartRenderers.Add(bodyPart.Key, spriteRenderer);
            }
        }
    }

    void OnMotionCaptureInput(GameEventArgs args)
    {
        MainThreadDispatcher.ExecuteInUpdate(() =>
        {
            string arg_key = args.FloatValue.ToString();

            if (bodyPartRenderers.ContainsKey(arg_key))
            {
                var spriteRenderer = bodyPartRenderers[arg_key];
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.red;
                }
            }

            foreach (var bodyPartRenderer in bodyPartRenderers)
            {
                if (bodyPartRenderer.Key == arg_key)
                {
                    continue;
                }

                if (bodyPartRenderer.Value != null)
                {
                    bodyPartRenderer.Value.color = Color.white;
                }
            }
        });
    }
}