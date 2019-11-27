using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    /// <summary>
    /// Game Events
    /// </summary>

    // When a line is created.
    public Action<DataPoint, DataPoint> OnLineCreated;
    
    // This is the action performed when a line is drawn, and contains a start and end accordingly.
    // It's more of an upate.
    public Action<DataPoint, DataPoint> OnLineUpdated;

    // When a line is termianted.
    public Action<DataPoint, DataPoint, DataEarnedScore> OnLineDestroyed;

    // When a tutorial swipe is done
    public Action OnCorrectTutorialSwipe;

    // Request a game state change
    public Action<GameState> OnGameStateChangeRequest;

    // The game state has been updated
    public Action<GameState> OnGameStateChange;

    // Whenever the state of bubbles changes, this is fired.
    public Action<List<DataPoint>> OnBubblesChange;

    // When the mouse is clicked
    public Action<DataPoint> OnClick;

    /// <summary>
    /// Settings
    /// </summary>

    // When the help section is toggled on or off in settings
    public Action<bool> OnShowHelpToggle;


    /// <summary>
    /// Internal
    /// </summary>

    // Serialization/internal event
    public Action<SerializedData> OnSerializedDataChange;

}
