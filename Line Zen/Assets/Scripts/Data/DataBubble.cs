using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BubbleType
{
    Standard = 0,
    Large,
}

public struct DataBubble
{
    private DataPoint position;     // The starting position and only position of a static point.
    private DataPoint targetOffset; // This is the location of the endpoint in relation to the position. If the position is (1,1) and offset is (0,1), then the offset position is (1,2). This is done so that a value of (0,0) means "no movement", not the origin.
    private float speed;            // This is how long it takes to complete one cycle of the oscilation between the position and targetOFfset;
    private BubbleType type;

    public DataBubble(DataPoint pos)
    {
        this.position = pos;
        this.targetOffset = new DataPoint(0, 0);
        this.speed = 0f;
        this.type = BubbleType.Standard;
    }

    public DataBubble(DataPoint pos, DataPoint target, float speed, BubbleType type)
    {
        this.position = pos;
        this.targetOffset = target;
        this.speed = speed;
        this.type = type;
    }

    public DataPoint GetPosition()
    {
        return position;
    } 

    public DataPoint OffsetPosition()
    {
        return position + targetOffset;
    }

    public BubbleType TypeOfBubble()
    {
        return type;
    }

    public float RawRadius()
    {
        float radius = GameCore.bubbleRadiusStandard;
        if (type == BubbleType.Large)
        {
            radius = GameCore.bubbleRadiusLarge;
        }

        return radius;
    }

    // Calculates the radius with leeway offset
    public float AdjustedRadius()
    {
        float radius = RawRadius();
        return radius + VisualLineManager.PLAYER_LINE_MAX_WIDTH / 2 + GameCore.widthLeeway;
    }
}
