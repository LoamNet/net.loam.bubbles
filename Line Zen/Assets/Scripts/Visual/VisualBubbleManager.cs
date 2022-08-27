using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBubbleManager : MonoBehaviour
{
    // Inspector Variables
    public Events events;
    public GameObject bubbleStandard;
    public GameObject bubbleLarge;

    // Internal Variables
    private GameObject fallback;

    // Start is called before the first frame update
    void Start()
    {
        // Construct fallback bubble
        fallback = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        fallback.name = "default bubble template";    
        GameObject.Destroy(fallback.GetComponent<SphereCollider>());
        Utils.SetMaterialToColorWithAlpha(fallback, new Color(.2f, .8f, 1f, .2f));
        fallback.SetActive(false);

        // Scale standard bubble
        bubbleStandard.transform.localScale = new Vector3(GameCore.bubbleRadiusStandard * 2, GameCore.bubbleRadiusStandard * 2, GameCore.bubbleRadiusStandard * 2);
        bubbleStandard.SetActive(false);
        bubbleLarge.transform.localScale = new Vector3(GameCore.bubbleRadiusStandard * 2, GameCore.bubbleRadiusStandard * 2, GameCore.bubbleRadiusStandard * 2);
        bubbleLarge.SetActive(false);
    }

    /// <summary>
    /// Makes a bubble game object
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public GameObject CreateBubble(DataBubble data)
    {
        BubbleType type = data.TypeOfBubble();
        string name = $"bubble {data.TypeOfBubble()}";
        GameObject toSpawn = bubbleStandard;
        if (type == BubbleType.Large)
        {
            toSpawn = bubbleLarge;
        }

        GameObject bubble = Instantiate(toSpawn, this.transform);
        VisualRemoveBubbleOnMenu vrbm = bubble.GetComponent<VisualRemoveBubbleOnMenu>();
        if(vrbm != null)
        {
            vrbm.events = events;
        }
        bubble.SetActive(true);
        bubble.name = name;
        bubble.transform.position = data.GetPosition();

        return bubble;
    }
}
