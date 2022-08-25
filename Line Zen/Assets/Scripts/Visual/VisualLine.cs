using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualLine
{
    private LineRenderer renderer;
    private VisualLineManager manager;

    public VisualLine(VisualLineManager manager, LineRenderer renderer, DataPoint start, DataPoint end, Color color, float thickness)
    {
        this.manager = manager;
        this.renderer = renderer;

        this.renderer.SetPosition(0, start);
        this.renderer.SetPosition(1, end);
        this.renderer.startColor = color;
        this.renderer.endColor = color;
        SetThickness(thickness);
    }

    public void SetThickness(float thickness)
    {
        const float minWidth = 0.01f;
        if (thickness <= minWidth)
        {
            thickness = minWidth;
        }

        this.renderer.startWidth = thickness;
        this.renderer.endWidth = thickness;
    }

    public void SetStart(DataPoint newPos)
    {
        this.renderer.SetPosition(0, newPos);
    }

    public void SetEnd(DataPoint newPos)
    {
        this.renderer.SetPosition(1, newPos);
    }

    public DataPoint Start()
    {
        return new DataPoint(this.renderer.GetPosition(0));
    }

    public DataPoint End()
    {
        return new DataPoint(this.renderer.GetPosition(1));
    }

    public void Destroy()
    {
        manager.StopTrackingLine(this);
        GameObject.Destroy(renderer.gameObject);
        manager = null;
    }
}
