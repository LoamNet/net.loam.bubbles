using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBubbleManager : MonoBehaviour
{
    public GameObject template;
    public static float bubbleRadius = 5.4f;

    // Start is called before the first frame update
    void Start()
    {
        if (template == null)
        { 
            template = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            template.name = "default bubble template";
            template.transform.localScale = new Vector3(bubbleRadius * 2, bubbleRadius * 2, bubbleRadius * 2);
            GameObject.Destroy(GetComponent<SphereCollider>());

            Utils.SetMaterialToColorWithAlpha(template, new Color(.2f, .6f, 1f, .3f));
        }

        template.SetActive(false);
    }

    public VisualBubble CreateBubble(DataPoint position)
    {
        GameObject bubble = Instantiate(template);
        bubble.SetActive(true);
        bubble.name = "bubble";
        bubble.transform.SetParent(this.transform);
        bubble.transform.position = position;

        return new VisualBubble(bubble, position);
    }
}
