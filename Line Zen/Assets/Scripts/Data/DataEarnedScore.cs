using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataEarnedScore
{
    public long score; // base score for an action
    public long bonus; // bonus for said action
    public long total; // sum of everythin else;
    public List<DataPoint> locations;

    public DataEarnedScore(int score, int bonus, List<DataPoint> locations)
    {
        this.score = score;
        this.bonus = bonus;
        this.total = score + bonus;
        this.locations = null;

        if (locations != null)
        {
            this.locations = new List<DataPoint>(locations.ToArray());
        }
    }
}

