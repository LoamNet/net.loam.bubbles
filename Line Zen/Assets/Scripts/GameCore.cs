using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    public Events events;
    public GameInputManager inputManager;
    public static float widthLeeway = 0.025f;

    private bool wasDownPreviously;
    private DataPoint lastLineStart;
    private DataPoint lastLineEnd;

    private List<DataPoint> bubbles;
    private GameState _internal_gm_st;
    private bool hasInit;

    public GameState State
    {
        get
        {
            return _internal_gm_st;
        }
        set
        {
            _internal_gm_st = value;
            events.OnGameStateChange?.Invoke(_internal_gm_st);
        } 
    }

    private void Start()
    {
        hasInit = false;
        bubbles = new List<DataPoint>();
        wasDownPreviously = false;

        events.OnGameStateChangeRequest += (state) => { State = state; };
    }

    private void Update()
    {
        if(!hasInit)
        {
            State = GameState.Startup;
            hasInit = true;
        }

        switch (State)
        {
            case GameState.Startup:
                break;
            case GameState.Options:
                break;
            case GameState.TutorialOne:
                UpdatePlayerLine();
                break;
            case GameState.GameUnlimited:
                UpdateBubbles();
                UpdatePlayerLine();
                break;
            case GameState.Exit:
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif 
                break;
        }
    }

    private void UpdateBubbles()
    {
        if(bubbles.Count < 6)
        {
            DataPoint screenSize = inputManager.ScreenSizeWorld();
            bubbles.Add(new DataPoint(
                Random.Range(-screenSize.X + VisualBubbleManager.bubbleRadius, screenSize.X - VisualBubbleManager.bubbleRadius), 
                Random.Range(-screenSize.Y + VisualBubbleManager.bubbleRadius * 4, screenSize.Y - VisualBubbleManager.bubbleRadius)));
            events.OnBubblesChange?.Invoke(bubbles);
        }
    }

    // Handles updating positions for the player line, along with line starting and finishing events.
    private void UpdatePlayerLine()
    {
        if (inputManager.PrimaryInputDown())
        {
            if (wasDownPreviously)
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
            if (wasDownPreviously)
            {
                events.OnLineDestroyed?.Invoke();
                wasDownPreviously = false;
            }
        }
    }
}
