using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Toggle))]
public class UIUpdateTutorialVariable : MonoBehaviour
{
    public Events events;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnityEngine.UI.Toggle>().onValueChanged.AddListener((newVal) => {
            events.OnTutorialToggle?.Invoke(newVal);
        });

        events.OnDataChanged += (data) => {
            GetComponent<UnityEngine.UI.Toggle>().isOn = data.showTutorial;
        };
    }
}
