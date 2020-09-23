using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBubble
{
    public DataPoint Position { get; private set; }
    public float Radius { get; private set; }
    public GameObject visual;

    public VisualBubble(GameObject visual, DataPoint position, float radius)
    {
        this.Position = new DataPoint(position.X, position.Y);
        this.Radius = radius;
        this.visual = visual;
    }

    public void Destroy()
    {
        GameObject.Destroy(visual);
        visual = null;
    }
}
