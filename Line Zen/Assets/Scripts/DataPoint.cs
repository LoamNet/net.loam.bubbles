using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataPoint
{
    private Vector2 pos; 

    public float X { get { return pos.x; } set { pos.x = value; } }
    public float Y { get { return pos.y; } set { pos.y = value; } }

    public DataPoint(float x = 0, float y = 0)
    {
        this.pos = new Vector2(x, y);
    }

    public DataPoint(DataPoint other)
    {
        this.pos = new Vector2(other.pos.x, other.pos.y);
    }

    public DataPoint(Vector2 other)
    {
        this.pos = new Vector2(other.x, other.y);
    }

    public DataPoint(Vector3 other)
    {
        this.pos = new Vector2(other.x, other.y);
    }

    public override string ToString()
    {
        return "[X]" + X + " [Y]" + Y;
    }

    public bool IsRealNumber()
    {
        return !float.IsNaN(X) && !float.IsNaN(Y);
    }

    /////////////////////////////////////////////////////
    // Operator overloading - implicit casting allowed //
    /////////////////////////////////////////////////////
    // Vector3 Unity Type
    public static implicit operator Vector3(DataPoint p)
    {
        return new Vector3(p.pos.x, p.pos.y, 0);
    }

    // Vector2 Unity Type
    public static implicit operator Vector2(DataPoint p)
    {
        return new Vector2(p.pos.x, p.pos.y);
    }
}
