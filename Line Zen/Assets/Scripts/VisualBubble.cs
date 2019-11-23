using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBubble
{
    public DataPoint Position { get; private set; }
    public GameObject visual;

    public VisualBubble(GameObject visual, DataPoint position)
    {
        this.Position = new DataPoint(position.X, position.Y);
        this.visual = visual;
    }

    public void Destroy()
    {
        GameObject.Destroy(visual);
        visual = null;
    }
}
