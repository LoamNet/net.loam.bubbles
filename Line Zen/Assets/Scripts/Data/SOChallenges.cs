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

        ValidateData();

        hasInit = true;
    }

    /// <summary>
    /// Ensures no duplicates or other issues.
    /// </summary>
    /// <returns></returns>
    private void ValidateData()
    {
        // Content here is run in editor

        // Below this point, skip when not in editor for perf reasons.
        bool isEditor = false;
#if UNITY_EDITOR
        isEditor = true;
#endif
        if(!isEditor)
        {
            return;
        }

        for (int i = 0; i < levels.Count; ++i)
        {
            for (int j = 0; j < levels.Count; ++j)
            {
                // skip same index
                if (i == j)
                {
                    continue;
                }

                if (levels[i].name == levels[j].name)
                {
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
#endif
                    throw new System.Exception($"Unsupported challenge list layout: Multiple entries with name {levels[i].name}");
                }
            }
        }
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
