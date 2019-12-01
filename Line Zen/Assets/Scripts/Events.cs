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
    public Action<GameState, GameMode> OnGameStateChangeRequest;

    // The game state has been updated
    public Action<GameState> OnGameStateChange;

    // Whenever the state of bubbles changes, this is fired.
    public Action<List<DataPoint>> OnBubblesChange;
    public Action<List<Tuple<DataPoint, DataPoint>>> OnGuideLinesChange;

    // When the mouse is clicked
    public Action<DataPoint> OnClick;

    public Action<DataPoint> OnBubbleDestroyed;

    /// <summary>
    /// Settings
    /// </summary>

    public Action<bool> OnShowHelpToggle;      // When the guide lines section is toggled on or off in settings
    public Action<bool> OnShowParticlesToggle; // When the particles visual is toggled on or off in settings.

    /// <summary>
    /// Internal
    /// </summary>

    // Serialization/internal event
    public Action<DataGeneral> OnDataChanged;

    // Clear
    public Action OnClearSavedData;

    public Action OnGameInitialized;

    public Action<string> OnNoSaveEntryFound;
}
