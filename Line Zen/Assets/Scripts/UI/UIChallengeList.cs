using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChallengeList : MonoBehaviour
{
    public Events events;
    public Data data;
    public GameObject parent;
    public UIChallengeEntry entryTemplate;
    public TMPro.TextMeshProUGUI fraction;
    public TMPro.TextMeshProUGUI percentage;
    public GameCore core;

    private List<UIChallengeEntry> entries;
    private SOChallenges list;

    public void Start()
    {
        entries = new List<UIChallengeEntry>();
        entryTemplate.gameObject.SetActive(false);

        events.OnGameStateChange += (state, mode) => {
            if (state == GameState.PickChallenge)
            {
                foreach (UIChallengeEntry entry in entries)
                {
                    Destroy(entry.gameObject);
                }

                entries.Clear();
                Initialize(list);
            }
        };
    }

    /// <summary>
    /// Hardcoded name acquire. Not very safe, but gets the job done.
    /// </summary>
    /// <param name="asset"></param>
    /// <returns></returns>
    public static string GetNameFromAsset(TextAsset asset)
    {
        return asset.text.Split(DataGeneral.contentSeparator)[0].Trim().Split(DataGeneral.keyValueSeparator)[1].Trim();
    }

    public void Initialize(SOChallenges challenges)
    {
        int count = challenges.Levels.Count;
        list = challenges;

        int starsTotal = count * 3;
        int starsCurrent = 0;

        // Parse out and add list items
        for (int i = 0; i < count; ++i)
        {
            TextAsset entry = challenges.Levels[i];
            UIChallengeEntry toParent = Instantiate(entryTemplate);

            string displayName = GetNameFromAsset(entry);

            toParent.events = events;

            // Parent accordingly
            toParent.transform.position = Vector3.zero;
            toParent.gameObject.transform.localScale = Vector3.one;
            toParent.transform.SetParent(parent.transform, false);

            // Parse out existing data, if present, and initialize entries.
            DataPuzzle? challenge = GetSavedDataChallenge(challenges, entry.name);
            int stars = 0;
            long score = 0;
            int group = -1;
            if (challenge.HasValue)
            {
                stars = challenge.Value.stars;
                score = challenge.Value.score;

                string content = core.levels.GetByName(challenge.Value.name).text;
                string tag = "group";
                int groupStart = content.IndexOf(tag);
                if (groupStart >= 0)
                {
                    string groupSection = content.Substring(groupStart + tag.Length);

                    int equalsStart = groupSection.IndexOf("=");
                    int lineEnd = groupSection.IndexOf("\n");
                    string groupVal;

                    if(lineEnd != -1)
                    {
                        groupVal = groupSection.Substring(equalsStart + 1, lineEnd);
                    }
                    else
                    {
                        groupVal = groupSection.Substring(equalsStart + 1);
                    }

                    group = int.Parse(groupVal);
                }

                starsCurrent += stars;
            }

            toParent.gameObject.SetActive(true);
            toParent.Initialize(entry.name, stars, displayName, score, group);
            entries.Add(toParent);
        }

        // Display data about stars
        fraction.text = starsCurrent.ToString() + " / " + starsTotal.ToString();
        percentage.text = Mathf.RoundToInt(((float)starsCurrent) / (float)starsTotal * 100).ToString() + "%";
    }

    // Search the saved data to see if a name of a level is there, and if so, get data on it.
    private DataPuzzle? GetSavedDataChallenge(SOChallenges challenges, string toGet)
    {
        DataGeneral dataGeneral = data.GetDataGeneral();
        string target = toGet.Trim().ToLowerInvariant();

        for (int i = 0; i < dataGeneral.challenges.Count; ++i)
        {
            string lower = dataGeneral.challenges[i].name.Trim().ToLowerInvariant();

            if (lower.Equals(target))
            {
                return dataGeneral.challenges[i];
            }
        }

        events.OnNoSaveEntryFound?.Invoke(target);
        return null;
    }
}
