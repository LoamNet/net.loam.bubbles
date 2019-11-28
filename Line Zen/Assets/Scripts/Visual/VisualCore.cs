using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCore : MonoBehaviour
{
    [Header("Base links")]
    public Events events;
    public GameCore core;
    public GameInputManager inputManager;

    [Header("Visual links")]
    public VisualLineManager lineManager;
    public VisualBubbleManager bubbleManager;
    public VisualScoreTextFeedback visualTextFeedback;

    [Header("Specific Visuals")]
    public GameObject lineEndCap; 

    // Internal private variables
    private VisualLine line;
    private List<VisualBubble> trackedBubbles;
    private List<VisualLine> trackedGuideLines;
    private List<GameObject> trackedGuideLineCaps;
    
    private void Awake()
    {
        line = null;
        trackedBubbles = new List<VisualBubble>();
        trackedGuideLines = new List<VisualLine>();
        trackedGuideLineCaps = new List<GameObject>();
    }

    public void Start()
    {
        // Line related gameplay events
        events.OnLineCreated += OnLineCreated;
        events.OnLineUpdated += OnLineDrawn;
        events.OnLineDestroyed += OnLineDestroyed;

        // Bubble related gameplay events
        events.OnBubblesChange += OnBubblesChange;
        events.OnGuideLinesChange += OnGuideLinesChange;
    }

    private void OnLineCreated(DataPoint start, DataPoint end)
    {
        line = lineManager.CreateLine(start, end);
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
            DataPoint s = inputManager.ConvertToScreenPoint(start);
            DataPoint e = inputManager.ConvertToScreenPoint(end);
            DataPoint worldPos = new DataPoint((s.X + e.X) / 2, (s.Y + e.Y) / 2);

            foreach (DataPoint point in points.locations)
            {
                DataPoint loc = inputManager.ConvertToScreenPoint(point);
                visualTextFeedback.CreateText(loc, "+" + GameCore.pointsPerBubble, TextType.ScoreAddition);
            }

            if (points.bonus != 0)
            {
                visualTextFeedback.CreateText(worldPos, "Bonus! +" + points.bonus, TextType.ScoreAdditionBonus);
            }
        }

        line = null;
    }

    private void OnBubblesChange(List<DataPoint> bubbles)
    {
        foreach (VisualBubble bubble in trackedBubbles)
        {
            bubble.Destroy();
        }

        trackedBubbles.Clear();

        foreach (DataPoint position in bubbles)
        {
            trackedBubbles.Add(bubbleManager.CreateBubble(position));
        }
    }

    private void OnGuideLinesChange(List<Tuple<DataPoint, DataPoint>> lines)
    {
        foreach (VisualLine line in trackedGuideLines)
        {
            line.Destroy();
        }

        trackedGuideLines.Clear();

        foreach (GameObject obj in trackedGuideLineCaps)
        {
            Destroy(obj);
        }

        trackedGuideLineCaps.Clear();

        foreach (Tuple<DataPoint, DataPoint> line in lines)
        {
            trackedGuideLines.Add(lineManager.CreateLine(line.Item1, line.Item2, new Color(.4f, .4f, .6f, .1f), .075f));

            GameObject endcap = Instantiate(lineEndCap, this.gameObject.transform);
            endcap.transform.position = line.Item1;
            VisualObjectFadeInDirection vofid = endcap.GetComponent<VisualObjectFadeInDirection>();
            if(vofid != null)
            {
                vofid.direction = (new Vector2(line.Item2.X - line.Item1.X, line.Item2.Y - line.Item1.Y)).normalized;
            }

            trackedGuideLineCaps.Add(endcap);
        }
    }

    private void Update()
    {
        DrawDebug();
    }


    private List<VisualLine> debugLines = new List<VisualLine>();
    private void DrawDebug()
    {
        foreach (VisualLine obj in debugLines)
        {
            obj.Destroy();
        }

        debugLines.Clear();

        if (core.Data.displayHelpLines)
        {
            if (line != null)
            {
                float lineWidth = .04f;

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
                            Color color = Color.HSVToRGB(1, .8f, .8f);

                            if (isIntermediate)
                            {
                                color = Color.HSVToRGB(.66f, .8f, .8f);
                            }
                            if (isHit)
                            {
                                color = Color.HSVToRGB(.33f, .8f, .8f);
                            }

                            VisualLine visual = lineManager.CreateLine(bubble.Position, closestPoint, color, lineWidth);
                            debugLines.Add(visual);
                        }
                    }
                }

                Vector2 directionUnscaled = new Vector2(line.End().X - line.Start().X, line.End().Y - line.Start().Y);
                Vector2 direction = directionUnscaled.normalized;
                Color col = new Color(.3f, .3f, .3f, .03f);
                float length = 50f;
                
                debugLines.Add(lineManager.CreateLine(line.Start(), line.Start() + new DataPoint(-direction * length), col, lineWidth));
                debugLines.Add(lineManager.CreateLine(line.End(), line.End() + new DataPoint(direction * length), col, lineWidth));
            }

            /*
             Screen boundary debugging
            DataPoint screenSize = inputManager.ScreenSizeWorld();
            debugLines.Add(lineManager.CreateLine(new DataPoint(-screenSize.X, screenSize.Y), new DataPoint(-screenSize.X, -screenSize.Y), Color.magenta, .1f)); // left
            debugLines.Add(lineManager.CreateLine(new DataPoint(screenSize.X, screenSize.Y), new DataPoint(screenSize.X, -screenSize.Y), Color.magenta, .1f)); // right
            debugLines.Add(lineManager.CreateLine(new DataPoint(-screenSize.X, screenSize.Y), new DataPoint(screenSize.X, screenSize.Y), Color.magenta, .1f)); // top
            debugLines.Add(lineManager.CreateLine(new DataPoint(-screenSize.X, -screenSize.Y), new DataPoint(screenSize.X, -screenSize.Y), Color.magenta, .1f)); // bottom
            */
        }
    }
}
