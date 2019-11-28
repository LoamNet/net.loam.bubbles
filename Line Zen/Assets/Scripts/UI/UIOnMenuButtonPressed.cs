using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class UIOnMenuButtonPressed : MonoBehaviour
{
    public Events events;

    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();

        button.onClick.AddListener(() => {
            events.OnLineDestroyed?.Invoke(new DataPoint(), new DataPoint(), new DataEarnedScore(0, 0, null));
        });
    }
}
