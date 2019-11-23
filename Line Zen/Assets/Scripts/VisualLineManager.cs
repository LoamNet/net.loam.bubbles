using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualLineManager : MonoBehaviour
{
    public GameObject template;
    public static float width = .2f;
    
    // Start is called before the first frame update
    void Start()
    {
        if(template == null)
        {
            const int capVertices = 10;

            template = new GameObject("default line template");
            LineRenderer renderer = template.AddComponent<LineRenderer>();

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);

            Material material = new Material(Shader.Find("UI/Unlit/Text"));
            material.SetTexture("_MainTex", texture);
            renderer.material = material;

            // Renderer settings
            renderer.startWidth = width;
            renderer.endWidth = width;
            renderer.numCapVertices = capVertices;
        }

        template.SetActive(false);
    }

    public VisualLine CreateLine(DataPoint start, DataPoint end, Color color, double thickness = .2)
    {
        GameObject line = Instantiate(template);
        line.name = "line";
        line.transform.SetParent(this.transform);
        line.SetActive(true);

        LineRenderer renderer = line.GetComponent<LineRenderer>();
        return new VisualLine(this, renderer, start, end, color, (float)thickness);
    }
}
