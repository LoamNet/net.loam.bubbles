using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    /// <summary>
    /// Game Events
    /// </summary>

    // Line and gameplay updates
    public Action<DataPoint, DataPoint> OnLineCreated;                    // When a line is created.
    public Action<DataPoint, DataPoint> OnLineUpdated;                    // This is the action performed when a line is drawn, and contains a start and end accordingly. It's more of an upate.
    public Action<DataPoint, DataPoint, DataEarnedScore> OnLineDestroyed; // When a line is termianted.
    public Action<GameState, GameMode> OnGameStateChangeRequest;          // Request a game state change
    public Action OnCorrectTutorialSwipe;                                 // When a tutorial swipe specifically dis done
    public Action<DataPoint> OnClick;                                     // When the primary input format is clicked

    // State updates
    public Action<GameState, GameMode> OnGameStateChange;                // The game state has been updated 
    public Action<List<DataBubble>> OnBubblesChange;                     // Whenever the state of bubbles changes, this is fired.
    public Action<List<Tuple<DataPoint, DataPoint>>> OnGuideLinesChange; // When one of the guide lines in the level visually is adjusted.
    public Action<DataPoint> OnBubbleDestroyed;                          // When a bubble is destroyed

    /// <summary>
    /// Settings
    /// </summary>

    // Menu toggles
    public Action<bool> OnShowHelpToggle;      // When the guide lines section is toggled on or off in settings
    public Action<bool> OnShowParticlesToggle; // When the particles visual is toggled on or off in settings.
    public Action<bool> OnTutorialToggle;      // Should we display the tutorial when starting a play session?
    public Action<bool> OnRequestPauseState;   // Requests a specific state of the pause menu

    /// <summary>
    /// Internal
    /// </summary>

    public Action<DataGeneral> OnDataChanged; // Serialization/internal event stating that something at some point in data we serialize is changed.
    public Action OnClearSavedData;           // When requesting the existing saved game data be cleared, post-confirm. At this point, we're positive we want it gone.
    public Action OnGameInitialized;          // When the game core itself is ready to go - happens AFTER start().
    public Action<string> OnNoSaveEntryFound; // When a level save isn't found when populating the level list
    public Action<string> OnLevelLoadRequest; // Contains name of the level to load. Dispatched when challenge level button/entry is pressed.
    public Action OnLevelReloadRequest;       // Requets a restart of the existing level
    public Action<bool> OnEnactPauseState;    // Set the state of the pause menu being visible.

    public Action<int> OnDebugScoreChange; // Debug score stuff, for level building.
}
