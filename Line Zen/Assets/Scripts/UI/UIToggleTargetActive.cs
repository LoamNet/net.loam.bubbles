using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class UIToggleTargetActive : MonoBehaviour
{
    // Links
    public GameObject toToggle;
    public Events events;

    // Internal Variables
    private Button button;

    // Hook up events
    private void OnEnable()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(ToggleActive);
    }

    // Disconnect Events
    private void OnDisable()
    {
        button.onClick.RemoveListener(ToggleActive);
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
