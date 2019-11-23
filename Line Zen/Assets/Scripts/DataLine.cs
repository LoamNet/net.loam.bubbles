using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLine
{
    private LineRenderer renderer;
    private VisualLineManager manager;

    public DataLine(VisualLineManager manager, LineRenderer renderer, DataPoint start, DataPoint end)
    {
        this.manager = manager;

        this.renderer = renderer;
        this.renderer.SetPosition(0, start);
        this.renderer.SetPosition(1, end);
    }

    public void SetStart(DataPoint newPos)
    {
        this.renderer.SetPosition(0, newPos);
    }

    public void SetEnd(DataPoint newPos)
    {
        this.renderer.SetPosition(1, newPos);
    }

    public void Destroy()
    {
        GameObject.Destroy(renderer.gameObject);
        manager = null;
    }
}
