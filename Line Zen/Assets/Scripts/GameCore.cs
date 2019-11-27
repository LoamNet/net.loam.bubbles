using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    // Setup and linsk
    public Events events;
    public GameInputManager inputManager;
    public SerializedDataIO dataIO;

    // Static values
    public static readonly float widthLeeway = 0.025f;
    public static readonly float bubbleRadius = .3f;
    public static readonly int bonusThreshold = 2;
    public static readonly int pointsPerBubble = 20;
    public static readonly int pointsPerBonusBubble = 15;

    // Private internal state
    private bool wasDownPreviously;
    private DataPoint lastLineStart;
    private DataPoint lastLineEnd;

    private List<DataPoint> bubbles;
    private GameState _internal_gm_st;
    private bool hasInit;

    private SerializedData data;
    public SerializedData Data { get { return data; } }

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

        events.OnShowHelpToggle += (isOn) => { data.displayHelpLines = isOn; };
        events.OnGameStateChangeRequest += (state) => { State = state; };
    }

    private void Update()
    {
        if(!hasInit)
        {
            data = dataIO.GetData();
            State = GameState.Startup;
            events.OnSerializedDataChange?.Invoke(data);
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
                Random.Range(-screenSize.X + bubbleRadius, screenSize.X - bubbleRadius), 
                Random.Range(-screenSize.Y + bubbleRadius * 4, screenSize.Y - bubbleRadius)));
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
                events.OnLineCreated?.Invoke(lastLineStart, lastLineEnd);
            }

            wasDownPreviously = true;
        }
        else
        {
            if (wasDownPreviously)
            {
                DataEarnedScore points = CollectBubblesAsNecessary();
                events.OnLineDestroyed?.Invoke(lastLineStart, lastLineEnd, points);
                wasDownPreviously = false;
            }
        }
    }

    // Returns how much the score changed by
    private DataEarnedScore CollectBubblesAsNecessary()
    {
        float triggerRadius = bubbleRadius + VisualLineManager.width / 2 + GameCore.widthLeeway;

        List<int> collectedIndexes = new List<int>();

        // Collect collisions
        for (int i = bubbles.Count - 1; i >= 0; --i)
        {
            DataPoint bubble = bubbles[i];

            bool isHit = Utils.IsLineTouchingCircle(lastLineStart, lastLineEnd, bubble, triggerRadius, bubbleRadius);
            
            if(isHit)
            {
                collectedIndexes.Add(i);
            }
        }

        // Score updating
        int hit = collectedIndexes.Count;
        int scoreBase = GameCore.pointsPerBubble * hit;
        int scoreBonus = 0;

        if(hit > bonusThreshold)
        {
            int bonusHits = hit - bonusThreshold;
            scoreBonus = bonusHits * bonusHits * pointsPerBonusBubble;
        }

        DataEarnedScore dataEarnedScore = new DataEarnedScore(scoreBase, scoreBonus);

        if (pointsPerBubble != 0)
        {
            data.score = data.score + dataEarnedScore.total;
            events.OnSerializedDataChange?.Invoke(data);
        }

        // Clear colleted bubbles
        foreach (int index in collectedIndexes)
        {
            bubbles.RemoveAt(index);
        }

        return dataEarnedScore;
    }
}
