using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIPauseMenu : MonoBehaviour
{
    public Events events;
    public Button buttonRestart;

    // Start is called before the first frame update
    void Awake()
    {
        events.OnEnactPauseState += (state) => {
            this.gameObject.SetActive(state);
        };

        buttonRestart.onClick.AddListener(() => {
            events.OnLevelReloadRequest?.Invoke();
        }); 
    }
}
