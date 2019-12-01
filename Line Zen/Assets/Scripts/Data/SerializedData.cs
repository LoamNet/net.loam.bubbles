using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DataGeneral
{
    public static readonly char keyValueSeparator = '=';
    public static readonly char valueSeparator = ',';
    public static readonly char contentSeparator = '\n';

    //////////////////////////////////////////////////////////
    public long score;
    public int level;
    public bool displayHelp;
    public bool displayParticles;
    public List<DataChallenge> challenges;
    //////////////////////////////////////////////////////////

    // Utility function for updating tracked level info.
    public void SetChallengeStats(string name, int score, bool onlyIncrease = true)
    {
        for (int i = 0; i < challenges.Count; ++i)
        {
            if(challenges[i].name.Equals(name))
            {
                if (!onlyIncrease || score > challenges[i].stars)
                {
                    challenges[i] = new DataChallenge(score, name);
                }

                return;
            }
        }
    }


    // Copy constructor
    public DataGeneral(DataGeneral other)
    {
        this.score = other.score;
        this.level = other.level;
        this.displayHelp = other.displayHelp;
        this.displayParticles = other.displayParticles;
        this.challenges = new List<DataChallenge>();

        if (other.challenges != null)
        {

            for(int i = 0; i < other.challenges.Count; ++i)
            {
                 this.challenges.Add(new DataChallenge(other.challenges[i]));
            }
        }
    }

    public static DataGeneral Defaults()
    {
        DataGeneral data;

        data.score = 0;
        data.level = 0;
        data.displayHelp = false;
        data.displayParticles = true;
        data.challenges = new List<DataChallenge>();

        return data;
    }

    public override string ToString()
    {
        string created = "";
        string sep = keyValueSeparator.ToString();
        string end = contentSeparator.ToString();

        created += "level" + sep + level.ToString() + end;
        created += "score" + sep + score.ToString() + end;
        created += "challenges" + sep + ListToString(challenges) + end;
        created += "displayhelp" + sep + displayHelp.ToString() + end;
        created += "displayParticles" + sep + displayParticles.ToString() + end;

        return created;
    }

    public string ListToString(List<DataChallenge> toWrite)
    {
        string str = "";

        for(int i = 0; i < toWrite.Count; ++i)
        {
            if (i != 0)
            {
                str += valueSeparator;
            }

            str += toWrite[i].ToString();
        }

        return str;
    }

    public List<DataChallenge> ListFromString(string toRead)
    {
        List<DataChallenge> parsed = new List<DataChallenge>();

        if(string.IsNullOrEmpty(toRead))
        {
            return parsed;
        }

        string[] split = toRead.Split(valueSeparator);

        for(int i = 0; i < split.Length; ++i)
        {
            DataChallenge dataChallenge = new DataChallenge();
            dataChallenge.FromString(split[i]);
            parsed.Add(dataChallenge);
        }

        return parsed;
    }

    public void FromString(string toParse)
    {
        string[] lines = toParse.Split(contentSeparator);
        for(int i = 0; i < lines.Length; ++i)
        {
            string[] line = lines[i].Split(keyValueSeparator);
            if(line.Length == 2)
            {
                try
                {
                    string cleaned = line[0].Trim().ToLowerInvariant();
                    switch(cleaned)
                    {
                        case "score":
                            score = long.Parse(line[1]);
                            break;
                        case "level":
                            level = int.Parse(line[1]);
                            break;
                        case "challenges":
                            challenges = ListFromString(line[1]);
                            break;
                        case "displayhelp":
                            displayHelp = bool.Parse(line[1]);
                            break;
                        case "displayparticles":
                            displayParticles = bool.Parse(line[1]);
                            break;
                    }
                }
                catch (System.Exception)
                {
                    Debug.LogError("Parsing error for line: " + line[0]);
                }
            }
        }
    }
}
