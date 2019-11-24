using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayableStates : MonoBehaviour
{
    public Events events;
    public List<GameState> displayDuring = new List<GameState>();

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            events.OnGameStateChange += SetVisibility;
        }
        catch
        {
            Debug.LogError("Failure in object " + gameObject.name);
        }
    }

    void SetVisibility(GameState state)
    {
        if (displayDuring.Contains(state))
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
