using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCore : MonoBehaviour
{
    [Header("Base links")]
    public Events events;
    public GameCore core;
    public GameInputManager input;


    [Header("Visual links")]
    public VisualLineManager lineManager;
    public VisualBubbleManager bubbleManager;
    public VisualScoreTextFeedback visualTextFeedback;

    private VisualLine line;
    private List<VisualBubble> trackedBubbles;

    private void Awake()
    {
        line = null;
        trackedBubbles = new List<VisualBubble>();
    }

    public void Start()
    {
        // Line related gameplay events
        events.OnLineCreated += OnLineCreated;
        events.OnLineUpdated += OnLineDrawn;
        events.OnLineDestroyed += OnLineDestroyed;

        // Bubble related gameplay events
        events.OnBubblesChange += OnBubblesChange;
    }

    private void OnLineCreated(DataPoint start, DataPoint end)
    {
        line = lineManager.CreateLine(start, end, Color.white);
    }

    private void OnLineDrawn(DataPoint start, DataPoint end)
    {
        line?.SetStart(start);
        line?.SetEnd(end);
    }

    private void OnLineDestroyed(DataPoint start, DataPoint end, DataEarnedScore points)
    {
        line?.Destroy();

        if (points.total > 0)
        {
            DataPoint s = input.ConvertToScreenPoint(start);
            DataPoint e = input.ConvertToScreenPoint(end);
            DataPoint worldPos = new DataPoint((s.X + e.X) / 2, (s.Y + e.Y) / 2);

            visualTextFeedback.CreateText(s, "+" + GameCore.pointsPerBubble, TextType.ScoreAddition);
            visualTextFeedback.CreateText(e, "+" + GameCore.pointsPerBubble, TextType.ScoreAddition);

            if (points.bonus != 0)
            {
                visualTextFeedback.CreateText(worldPos, "Bonus! +" + points.bonus, TextType.ScoreAdditionBonus);
            }
        }

        line = null;
    }

    private void OnBubblesChange(List<DataPoint> bubbles)
    {
        foreach(VisualBubble bubble in trackedBubbles)
        {
            bubble.Destroy();
        }

        trackedBubbles.Clear();

        foreach(DataPoint position in bubbles)
        {
            trackedBubbles.Add(bubbleManager.CreateBubble(position));
        }
    }

    private void Update()
    {
        DrawDebug();
    }


    private List<VisualLine> debugLines = new List<VisualLine>();
    private void DrawDebug()
    {
        foreach(VisualLine obj in debugLines)
        {
            obj.Destroy();
        }

        debugLines.Clear();

        if (core.Data.displayHelpLines)
        {
            if (line != null)
            {
                foreach (VisualBubble bubble in trackedBubbles)
                {
                    if (bubble.visual != null)
                    {
                        DataPoint closestPoint = Utils.GetClosestPointOnLine(line.Start(), line.End(), bubble.Position);
                        float triggerRadius = GameCore.bubbleRadius + VisualLineManager.width / 2 + GameCore.widthLeeway;

                        bool isHit = Utils.IsLineTouchingCircle(line.Start(), line.End(), bubble.Position, triggerRadius, GameCore.bubbleRadius);
                        bool isIntermediate = Utils.IsInRadiusLineRange(line.Start(), line.End(), bubble.Position, triggerRadius);

                        if (closestPoint.IsRealNumber())
                        {
                            VisualLine visual = lineManager.CreateLine(bubble.Position, closestPoint, isHit ? Color.green : (isIntermediate ? Color.blue : Color.red), .04f);
                            debugLines.Add(visual);
                        }
                    }
                }
            }
        }
    }
}
