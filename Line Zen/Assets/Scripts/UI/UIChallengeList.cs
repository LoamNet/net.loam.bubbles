using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIChallengeList : MonoBehaviour
{
    public Events events;
    public Data data;
    public GameObject parent;
    public UIChallengeEntry entryTemplate;

    private List<UIChallengeEntry> entries;
    private SOChallengeList list;

    public void Start()
    {
        entries = new List<UIChallengeEntry>();
        entryTemplate.gameObject.SetActive(false);

        events.OnGameStateChange += (state) => {
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

    public void Initialize(SOChallengeList challenges)
    {
        int count = challenges.levels.Count;
        list = challenges;

        for (int i = 0; i < count; ++i)
        {
            UIChallengeEntry toParent = Instantiate(entryTemplate);
            TextAsset entry = challenges.levels[i];

            ////////////////////////////
            // Temporary name acqurie //
            ////////////////////////////
            
            string displayName = entry.text.Split(DataGeneral.contentSeparator)[0].Trim().Split(DataGeneral.keyValueSeparator)[1].Trim();

            ////////////////////////////
            
            toParent.events = events;

            // Parent accordingly
            toParent.transform.position = Vector3.zero;
            toParent.gameObject.transform.localScale = Vector3.one;
            toParent.transform.SetParent(parent.transform, false);

            // Parse out existing data, if present, and initialize entries.
            DataChallenge? challenge = GetSavedDataChallenge(challenges, entry.name);
            int stars = 0;
            if (challenge.HasValue)
            {
                stars = challenge.Value.stars;
            }

            toParent.gameObject.SetActive(true);
            toParent.Initialize(entry.name, stars, displayName);
            entries.Add(toParent);
        }
    }

    // Search the saved data to see if a name of a level is there, and if so, get data on it.
    private DataChallenge? GetSavedDataChallenge(SOChallengeList challenges, string toGet)
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
