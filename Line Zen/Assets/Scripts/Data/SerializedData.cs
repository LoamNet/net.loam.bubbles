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
    public int seed;
    public long score;
    public int level;
    public bool showTutorial;
    public bool showHelp;
    public bool showParticles;
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

    /// <summary>
    /// Nullable, will return either the challenge found or null if not found
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool TryGetChallenge(string name, out DataChallenge data)
    {
        data = default;

        for (int i = 0; i < challenges.Count; ++i)
        {
            if(challenges[i].name.Equals(name))
            {
                data = challenges[i];
                return true;
            }
        }

        return false;
    }

    // Copy constructor
    public DataGeneral(DataGeneral other)
    {
        this.seed = other.seed;
        this.score = other.score;
        this.level = other.level;
        this.showTutorial = other.showTutorial;
        this.showHelp = other.showHelp;
        this.showParticles = other.showParticles;
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

        data.seed = Random.Range(1, int.MaxValue);
        data.score = 0;
        data.level = 0;
        data.showTutorial = true;
        data.showHelp = false;
        data.showParticles = true;
        data.challenges = new List<DataChallenge>();

        return data;
    }

    private string Line(string label, object value)
    {
        // String interpolation simplifies .ToString() calls on all values used.
        return $"{label}{keyValueSeparator}{value}{contentSeparator}";
    }

    public override string ToString()
    {
        string created = "";


        created += Line("seed", seed);
        created += Line("level", level);
        created += Line("score", score);
        created += Line("challenges", ListToString(challenges));
        created += Line("showTutorial", showTutorial);
        created += Line("showHelp", showHelp);
        created += Line("showParticles", showParticles);

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
                    string value = line[1];

                    switch(cleaned)
                    {
                        case "seed":
                            seed = int.Parse(value);
                            break;
                        case "score":
                            score = long.Parse(value);
                            break;
                        case "level":
                            level = int.Parse(value);
                            break;
                        case "challenges":
                            challenges = ListFromString(value);
                            break;
                        case "showtutorial":
                            showTutorial = bool.Parse(value);
                            break;
                        case "showhelp":
                            showHelp = bool.Parse(value);
                            break;
                        case "showparticles":
                            showParticles = bool.Parse(value);
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
