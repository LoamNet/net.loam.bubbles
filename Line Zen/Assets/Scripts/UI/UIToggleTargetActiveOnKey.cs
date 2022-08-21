using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIToggleTargetActiveOnKey : MonoBehaviour
{
    // Links
    public GameObject toToggle;
    public Events events;
    public KeyCode key;

    // Internal Variables
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();   
    }

    private void Update()
    {
        if (Input.GetKeyDown(key))
        {
            ToggleActive();
        }
    }

    private void ToggleActive()
    {
        bool targetState = !toToggle.activeInHierarchy;

        if (toToggle.GetComponent<UIPauseMenu>() != null)
        {
            events?.OnRequestPauseState?.Invoke(targetState);
        }
        else
        {
            toToggle.SetActive(targetState);
        }
    }
}
