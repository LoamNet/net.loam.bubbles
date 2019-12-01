using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenges", menuName = "Loam/Challenge List", order = 1)]
public class SOChallengeList : ScriptableObject
{
    public List<TextAsset> levels = new List<TextAsset>();

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
