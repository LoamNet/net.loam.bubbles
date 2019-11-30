using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UIUpdateScoreVariable : MonoBehaviour
{
    public Events events;

    // Start is called before the first frame update
    void Start()
    {
        events.OnDataChanged += (data) => {
            GetComponent<TMPro.TextMeshProUGUI>().text = (data.score > 0 ? "" + data.score : "Nothing Yet!");
        };
    }
}
