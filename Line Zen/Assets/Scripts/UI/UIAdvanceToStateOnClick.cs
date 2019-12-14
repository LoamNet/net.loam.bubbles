using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Button))]
public class UIAdvanceToStateOnClick : MonoBehaviour
{
    public Events events;
    public GameState state;
    public GameMode mode;
    public List<KeyCode> triggerKeys;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => {
            events.OnGameStateChangeRequest?.Invoke(state, mode);
        });
    }

    private void Update()
    {
        if(this.gameObject.activeInHierarchy)
        {
            if (triggerKeys != null)
            {
                foreach (KeyCode key in triggerKeys)
                {
                    if(Input.GetKeyDown(key))
                    {
                        events.OnGameStateChangeRequest?.Invoke(state, mode);
                        break;
                    }
                }
            }
        }
    }
}
