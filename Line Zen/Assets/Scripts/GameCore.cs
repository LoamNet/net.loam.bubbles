using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    public Events events;
    public GameInputManager inputManager;

    private bool wasDownPreviously;
    private DataPoint lastLineStart;
    private DataPoint lastLineEnd;

    private void Start()
    {
        wasDownPreviously = false;
    }

    private void Update()
    {
        if (inputManager.PrimaryInputDown())
        {
            if(wasDownPreviously)
            {
                lastLineEnd = inputManager.PrimaryInputPosWorld();
                events.OnLineUpdated?.Invoke(lastLineStart, lastLineEnd);
            }
            else
            {
                lastLineStart = inputManager.PrimaryInputPosWorld();
                lastLineEnd = inputManager.PrimaryInputPosWorld();
                events.OnLineCreated?.Invoke(lastLineEnd, lastLineEnd);
            }

            wasDownPreviously = true;
        }
        else
        {
            if(wasDownPreviously)
            {
                events.OnLineDestroyed?.Invoke();
                wasDownPreviously = false;
            }
        }
    }
}
