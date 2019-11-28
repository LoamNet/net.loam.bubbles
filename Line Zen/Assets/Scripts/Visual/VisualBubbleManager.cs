using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBubbleManager : MonoBehaviour
{
    public Events events;
    public GameObject template;
    
    // Start is called before the first frame update
    void Start()
    {
        if (template == null)
        { 
            template = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            template.name = "default bubble template";    
            GameObject.Destroy(GetComponent<SphereCollider>());

            Utils.SetMaterialToColorWithAlpha(template, new Color(.2f, .8f, 1f, .2f));
        }

        template.transform.localScale = new Vector3(GameCore.bubbleRadius * 2, GameCore.bubbleRadius * 2, GameCore.bubbleRadius * 2);
        template.SetActive(false);
    }

    public VisualBubble CreateBubble(DataPoint position)
    {
        GameObject bubble = Instantiate(template);
        VisualRemoveBubbleOnMenu vrbm = bubble.GetComponent<VisualRemoveBubbleOnMenu>();
        if(vrbm != null)
        {
            vrbm.events = events;
        }
        bubble.SetActive(true);
        bubble.name = "bubble";
        bubble.transform.SetParent(this.transform);
        bubble.transform.position = position;

        return new VisualBubble(bubble, position);
    }
}
