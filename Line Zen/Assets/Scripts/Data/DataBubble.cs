using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataBubble
{
    private DataPoint position;
    private DataPoint targetOffset;
    private float speed;

    public DataBubble(DataPoint pos)
    {
        this.position = pos;
        this.targetOffset = new DataPoint(0, 0);
        this.speed = 0f;
    }

    public DataBubble(DataPoint pos, DataPoint target, float speed)
    {
        this.position = pos;
        this.targetOffset = target;
        this.speed = speed;
    }

    public DataPoint GetPosition()
    {
        return position;
    }
}
