﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCore : MonoBehaviour
{
    [Header("Base links")]
    public Data data;
    public Events events;
    public GameCore core;
    public GameInputManager inputManager;

    [Header("Visual links")]
    public VisualLineManager lineManager;
    public VisualBubbleManager bubbleManager;
    public VisualScoreTextFeedback visualTextFeedback;
    public VisualParticleManager particleManager;
    public UIChallengeList challengeList;

    [Header("Specific Visuals")]
    public GameObject lineEndCap;
    public float lineThickeningSpeedScalar = 1;
    public AnimationCurve curve;

    [Header("Colors")]
    public Color guideLineColor = new Color(.4f, .4f, .6f, .1f);

    // Internal private variables
    private VisualLine line;
    private float lineVisualWidth = 0;
    private List<Tuple<DataBubble, GameObject>> trackedBubbles;
    private List<VisualLine> trackedGuideLines;
    private List<GameObject> trackedGuideLineCaps;
    
    private void Awake()
    {
        line = null;
        trackedBubbles = new List<Tuple<DataBubble, GameObject>>();
        trackedGuideLines = new List<VisualLine>();
        trackedGuideLineCaps = new List<GameObject>();
    }

    public void Start()
    {
        // Line related gameplay events
        events.OnLineCreated += OnLineCreated;
        events.OnLineUpdated += OnLineUpdate;
        events.OnLineDestroyed += OnLineDestroyed;

        // Bubble related gameplay events
        events.OnBubblesChange += OnBubblesChange;
        events.OnGuideLinesChange += OnGuideLinesChange;
        events.OnBubbleDestroyed += OnBubbleDestroyed;

        events.OnGameInitialized += () => {
            if (challengeList != null)
            {
                challengeList.Initialize(core.ChallengeLevels);
            }
            else
            {
                Debug.LogWarning("No challenge list was specified, and so no challenge list could be initialized!");
            }
        };
    }

    private void OnBubbleDestroyed(DataPoint pos)
    {
        particleManager.CreateBubbleExplosion(pos);
    }

    private void OnLineCreated(DataPoint start, DataPoint end)
    {
        line = lineManager.CreateLine(start, end, null, 0, 100);
        lineVisualWidth = 0;
    }

    private void OnLineUpdate(DataPoint start, DataPoint end)
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
        lineVisualWidth = 0;
    }

    // When bubbles change in any way, redraw them all at their new positions.
    private void OnBubblesChange(List<DataBubble> bubbles)
    {
        foreach (Tuple<DataBubble, GameObject> VisualBubbleTuple in trackedBubbles)
        {
            GameObject.Destroy(VisualBubbleTuple.Item2);
        }

        trackedBubbles.Clear();

        foreach (DataBubble bubble in bubbles)
        {
            Tuple<DataBubble, GameObject> bundle = 
                new Tuple<DataBubble, GameObject>(bubble, bubbleManager.CreateBubble(bubble));
            trackedBubbles.Add(bundle);
        }
    }

    // Adjust the location of any guidelines being drawn
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
            trackedGuideLines.Add(lineManager.CreateLine(line.Item1, line.Item2, guideLineColor, .075f));

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

        if (line != null)
        {
            lineVisualWidth += Time.deltaTime * lineThickeningSpeedScalar;
            Mathf.Clamp(lineVisualWidth, 0, VisualLineManager.PLAYER_LINE_MAX_WIDTH);

            float t = lineVisualWidth / VisualLineManager.PLAYER_LINE_MAX_WIDTH;
            float adjusted = curve.Evaluate(t);

            line?.SetThickness(adjusted * VisualLineManager.PLAYER_LINE_MAX_WIDTH);
        }


        List<Tuple<DataBubble, GameObject>> toDispose = new List<Tuple<DataBubble, GameObject>>();
        // Update positions
        foreach (Tuple<DataBubble, GameObject> entry in trackedBubbles)
        {
            if (entry.Item2 == null)
            {
                toDispose.Add(entry);
            }
            else
            {
                entry.Item2.transform.position = entry.Item1.GetPosition();
            }
        }

        if(toDispose.Count > 0)
        {
            int count = toDispose.Count;
            foreach(Tuple<DataBubble, GameObject> entry in toDispose)
            {
                trackedBubbles.Remove(entry);
            }
            Debug.Log($"Update disposed of {count} entries in trackedBubbles");
        }
    }


    // Debug lines are used to determine the behavior once the input is released for the bubbles present.
    private List<VisualLine> debugLines = new List<VisualLine>();
    private void DrawDebug()
    {
        foreach (VisualLine obj in debugLines)
        {
            obj.Destroy();
        }

        debugLines.Clear();

        if (data.GetDataGeneral().showHelp)
        {
            if (line != null)
            {
                float lineWidth = .04f;

                // Take every bubble and draw debug lines from it to the line to help display what's going on.
                foreach (Tuple<DataBubble, GameObject> bubbleItem in trackedBubbles)
                {
                    if (bubbleItem.Item2 != null)
                    {
                        DataPoint closestPoint = Utils.GetClosestPointOnLine(line.Start(), line.End(), bubbleItem.Item1.GetPosition());
                        float triggerRadius = bubbleItem.Item1.AdjustedRadius(); // Eh?

                        bool isHit = Utils.IsLineTouchingCircle(line.Start(), line.End(), bubbleItem.Item1.GetPosition(), triggerRadius, GameCore.bubbleRadiusStandard);
                        bool isIntermediate = Utils.IsInRadiusLineRange(line.Start(), line.End(), bubbleItem.Item1.GetPosition(), triggerRadius);

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

                            VisualLine visual = lineManager.CreateLine(bubbleItem.Item1.GetPosition(), closestPoint, color, lineWidth);
                            debugLines.Add(visual);
                        }
                    }

                    if (bubbleItem.Item1.AdjustedRadius() > (GameCore.bubbleRadiusStandard + VisualLineManager.PLAYER_LINE_MAX_WIDTH / 2 + GameCore.widthLeeway) + 0.001f)
                    {
                        DataPoint[] pos = GameCore.DetermineSplits(new DataBubble(bubbleItem.Item1.GetPosition(), new DataPoint(), speed: 0, BubbleType.Large), line.Start(), line.End());
                        foreach (DataPoint point in pos)
                        {
                            debugLines.AddRange(lineManager.CreatePlus(point, Color.white));
                        }
                    }
                }


                // Draw extensions on the line itself
                Vector2 directionUnscaled = new Vector2(line.End().X - line.Start().X, line.End().Y - line.Start().Y);
                Vector2 direction = directionUnscaled.normalized;
                Color col = new Color(.3f, .3f, .3f, .03f);
                float length = 50f;
                
                debugLines.Add(lineManager.CreateLine(line.Start(), line.Start() + new DataPoint(-direction * length), col, lineWidth));
                debugLines.Add(lineManager.CreateLine(line.End(), line.End() + new DataPoint(direction * length), col, lineWidth));
            }

            
            // Screen boundary debugging
            DataPoint screenSize = inputManager.ScreenSizeWorld();
            Color borderColor = new Color(1f, .7f, .8f);
            float borderThickness = 0.1f;
            debugLines.Add(lineManager.CreateLine(new DataPoint(-screenSize.X, screenSize.Y), new DataPoint(-screenSize.X, -screenSize.Y), borderColor, borderThickness)); // left
            debugLines.Add(lineManager.CreateLine(new DataPoint(screenSize.X, screenSize.Y), new DataPoint(screenSize.X, -screenSize.Y), borderColor, borderThickness)); // right
            debugLines.Add(lineManager.CreateLine(new DataPoint(-screenSize.X, screenSize.Y), new DataPoint(screenSize.X, screenSize.Y), borderColor, borderThickness)); // top
            debugLines.Add(lineManager.CreateLine(new DataPoint(-screenSize.X, -screenSize.Y), new DataPoint(screenSize.X, -screenSize.Y), borderColor, borderThickness)); // bottom
            
        }
    }
}
