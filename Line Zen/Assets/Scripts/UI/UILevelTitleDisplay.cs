using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILevelTitleDisplay : MonoBehaviour
{
    public Events events;
    public GameCore core;
    private TextAsset level = null;

    /// <summary>
    /// Very gross polling
    /// </summary>
    void Update()
    {
        if (core != null && core.CurrentLevel != null)
        {
            if (level != core.CurrentLevel)
            {
                level = core.CurrentLevel;
                string name = UIChallengeList.GetNameFromAsset(level);
                GetComponent<TMPro.TextMeshProUGUI>().text = name;
            }
        }
    }
}

