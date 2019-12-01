using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Challenges", menuName = "Loam/Challenge List", order = 1)]
public class SOChallengeList : ScriptableObject
{
    public List<TextAsset> levels = new List<TextAsset>();
}
