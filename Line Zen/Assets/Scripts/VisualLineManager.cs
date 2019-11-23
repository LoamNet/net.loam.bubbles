using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisualLineManager : MonoBehaviour
{
    GameObject template;

    // Start is called before the first frame update
    void Start()
    {
        if(template == null)
        {
            const float width = .2f;
            const int capVertices = 10;

            template = new GameObject("default line template");
            LineRenderer renderer = template.AddComponent<LineRenderer>();

            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.white);

            Material material = new Material(Shader.Find("Standard"));
            material.SetTexture("_MainTex", texture);
            renderer.material = material;

            // Renderer settings
            renderer.startWidth = width;
            renderer.endWidth = width;
            renderer.numCapVertices = capVertices;
        }

        template.SetActive(false);
    }

    public DataLine CreateLine(DataPoint start, DataPoint end)
    {
        GameObject line = Instantiate(template);
        line.SetActive(true);
        line.name = "line";
        line.transform.SetParent(this.transform);

        LineRenderer renderer = line.GetComponent<LineRenderer>();
        renderer.SetPosition(0, start);
        renderer.SetPosition(1, end);

        return new DataLine(this, renderer, start, end);
    }
}
