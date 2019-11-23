using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCore : MonoBehaviour
{
    public Events events;
    public VisualLineManager lineManager;

    private DataLine line; 

    public void Start()
    {
        events.OnLineCreated += OnLineCreated;
        events.OnLineUpdated += OnLineDrawn;
        events.OnLineDestroyed += OnLineDestroyed;
    }

    public void OnLineCreated(DataPoint start, DataPoint end)
    {
        line = lineManager.CreateLine(start, end);
    }

    public void OnLineDrawn(DataPoint start, DataPoint end)
    {
        line?.SetStart(start);
        line?.SetEnd(end);
    }

    public void OnLineDestroyed()
    {
        line?.Destroy();
        line = null;
    }

}
