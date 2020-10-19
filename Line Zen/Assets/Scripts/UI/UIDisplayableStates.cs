using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDisplayableStates : MonoBehaviour
{
    public Events events;
    public List<GameState> displayDuring = new List<GameState>();
    public GameMode selectiveMode = GameMode.Undefined;

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

    private void OnDestroy()
    {
        events.OnGameStateChange -= SetVisibility;
    }

    void SetVisibility(GameState state, GameMode mode)
    {
        if (displayDuring.Contains(state))
        {
            if (this.gameObject as UnityEngine.GameObject != null)
            {
                if (selectiveMode != GameMode.Undefined)
                {
                    if (selectiveMode == mode)
                    {
                        this.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.gameObject.SetActive(false);
                    }
                }
                else
                {
                    this.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            if (this.gameObject as UnityEngine.GameObject != null)
                this.gameObject.SetActive(false);
        }
    }
}
