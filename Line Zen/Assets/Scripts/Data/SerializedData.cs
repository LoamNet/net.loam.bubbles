using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct SerializedData
{
    public static readonly char keyValueSeparator = '=';
    public static readonly char contentSeparator = '\n';

    public long score;
    public bool displayHelpLines;
    public int level;

    public override string ToString()
    {
        string created = "";
        string sep = keyValueSeparator.ToString();
        string end = contentSeparator.ToString();

        created += "level" + sep + level.ToString() + end;
        created += "score" + sep + score.ToString() + end;
        created += "displayHelp" + sep + displayHelpLines.ToString() + end;

        return created;
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
                        case "displayhelp":
                            displayHelpLines = bool.Parse(line[1]);
                            break;
                        case "level":
                            level = int.Parse(line[1]);
                            break;
                    }
                }
                catch (System.Exception)
                {
                    Debug.LogError("Parsing error for line: " + line);
                }
            }
        }
    }
}
