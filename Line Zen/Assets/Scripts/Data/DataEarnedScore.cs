using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataEarnedScore
{
    public long score; // base score for an action
    public long bonus; // bonus for said action
    public long total; // sum of everythin else;

    public DataEarnedScore(int score, int bonus)
    {
        this.score = score;
        this.bonus = bonus;
        this.total = score + bonus;
    }
}

