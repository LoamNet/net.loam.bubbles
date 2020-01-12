using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataBubble
{
    private DataPoint position;     // The starting position and only position of a static point.
    private DataPoint targetOffset; // This is the location of the endpoint in relation to the position. If the position is (1,1) and offset is (0,1), then the offset position is (1,2). This is done so that a value of (0,0) means "no movement", not the origin.
    private float speed;            // This is how long it takes to complete one cycle of the oscilation between the position and targetOFfset;
     
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

    public DataPoint GetOffsetPosition()
    {
        return position + targetOffset;
    }
}
