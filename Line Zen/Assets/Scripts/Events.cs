using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    // When a line is created.
    public Action<DataPoint, DataPoint> OnLineCreated;
    
    // This is the action performed when a line is drawn, and contains a start and end accordingly.
    // It's more of an upate.
    public Action<DataPoint, DataPoint> OnLineUpdated;

    // When a line is termianted.
    public Action OnLineDestroyed;

    // When a tutorial swipe is done
    public Action OnCorrectTutorialSwipe;

    // Request a game state change
    public Action<GameState> OnGameStateChangeRequest;

    // The game state has been updated
    public Action<GameState> OnGameStateChange;

    // Whenever the state of bubbles changes, this is fired.
    public Action<List<DataPoint>> OnBubblesChange;

    public Action<SerializedData> OnSerializedDataChange;

    public Action<bool> OnShowHelpToggle;
}
