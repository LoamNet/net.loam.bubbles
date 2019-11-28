using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCore : MonoBehaviour
{
    // Setup and linsk
    [Header("Base links")]
    public Events events;
    public GameInputManager inputManager;
    public SerializedDataIO dataIO;
    // Static values
    public static readonly float widthLeeway = 0.025f;
    public static readonly float bubbleRadius = .3f;
    public static readonly int bonusThreshold = 2;
    public static readonly int pointsPerBubble = 20;
    public static readonly int pointsPerBonusBubble = 15;

    [Header("State")]
    public GameState internalState;
    public TextAsset currentLevel;

    // Private internal state
    private bool wasDownPreviously;
    private DataPoint lastLineStart;
    private DataPoint lastLineEnd;

    private List<DataPoint> bubbles;
    private bool hasInit;

    private SerializedData data;
    public SerializedData Data { get { return data; } }


    bool internalStateCurrentHasInit;
    public GameState State
    {
        get
        {
            return internalState;
        }
        set
        {
            internalState = value;
            internalStateCurrentHasInit = false;
            events.OnGameStateChange?.Invoke(internalState);
        } 
    }

    private void Start()
    {
        hasInit = false;
        internalStateCurrentHasInit = false;
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
            events.OnGameStateChange?.Invoke(internalState);
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
            case GameState.TutorialTwo:
                UpdatePlayerLine();
                break;
            case GameState.GameLoadLevel:
                PopulateLevelBubbles(currentLevel);
                UpdatePlayerLine();
                CheckIfDoneLevelBubbles();
                break;
            case GameState.GameUnlimited:
                PopulateUnlimitedBubbles();
                UpdatePlayerLine();
                CheckIfDoneUnlimitedBubbles();
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

    public void PopulateLevelBubbles(TextAsset levelData)
    {
        if(!internalStateCurrentHasInit)
        {
            internalStateCurrentHasInit = true;
        }
    }

    private void CheckIfDoneLevelBubbles()
    {
        if (currentLevel == null)
        {
            Debug.LogWarning("No level was present, swapping to unlimited!");
            State = GameState.GameUnlimited;
            return;
        }

        if (bubbles.Count < 1)
        {
            State = GameState.GameLoadLevel;
            return;
        }
    }

    private void CheckIfDoneUnlimitedBubbles()
    {
        if (bubbles.Count < 1)
        {
            State = GameState.GameUnlimited;
            return;
        }
    }

    private void PopulateUnlimitedBubbles()
    {
        if (!internalStateCurrentHasInit)
        {
            while (bubbles.Count < 7)
            {
                DataPoint screenSize = inputManager.ScreenSizeWorld();
                bubbles.Add(new DataPoint(
                    Random.Range(-screenSize.X + bubbleRadius, screenSize.X - bubbleRadius),
                    Random.Range(-screenSize.Y + bubbleRadius * 4, screenSize.Y - bubbleRadius)));
                events.OnBubblesChange?.Invoke(bubbles);
            }

            internalStateCurrentHasInit = true;
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
                events.OnBubblesChange?.Invoke(bubbles);
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
