using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DataChallenge
{
    public int stars;
    public string name;

    public static readonly char separator = ':';

    public DataChallenge(DataChallenge other)
    {
        this.stars = other.stars;
        this.name = other.name;
    }

    public DataChallenge(int stars, string name)
    {
        this.stars = stars;
        this.name = name;
    }

    public override string ToString()
    {
        return name.ToString() + separator.ToString() + stars.ToString();
    }

    public void FromString(string toParse)
    {
        string[] split = toParse.Split(separator);

        name = split[0].Trim();
        stars = int.Parse(split[1].Trim());
    }
}
