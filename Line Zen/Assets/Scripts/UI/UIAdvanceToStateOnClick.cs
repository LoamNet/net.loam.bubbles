using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class UIAdvanceToStateOnClick : MonoBehaviour
{
    public Events events;
    public GameState state;
    public GameMode mode;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            events.OnGameStateChangeRequest?.Invoke(state, mode);
        });
    }
}
