using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenges", menuName = "Loam/Challenges Collection", order = 2)]
public class SOChallenges : ScriptableObject
{
    // Inspector
    [SerializeField] private SOChallengeList basic;
    [SerializeField] private SOChallengeList large; 
    [SerializeField] private SOChallengeList moving;
    [SerializeField] private SOChallengeList varied;

    // External
    [System.NonSerialized] private List<TextAsset> levels = new List<TextAsset>();
    public List<TextAsset> Levels { get { return levels; } }

    // Internal
    [System.NonSerialized] private bool hasInit = false;

    public void Initialize()
    {
        if (hasInit)
        {
            return;
        }

        levels.AddRange(basic.entries);
        levels.AddRange(large.entries);
        levels.AddRange(moving.entries);
        levels.AddRange(varied.entries);

        hasInit = true;
    }

    public TextAsset GetByName(string name)
    {
        for (int i = 0; i < levels.Count; ++i)
        {
            if (levels[i].name.Equals(name))
            {
                return levels[i];
            }
        }

        return null;
    }
}
