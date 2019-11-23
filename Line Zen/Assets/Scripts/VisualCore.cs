using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCore : MonoBehaviour
{
    public Events events;
    public VisualLineManager lineManager;
    public VisualBubbleManager bubbleManager;

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

    private void OnLineDestroyed()
    {
        line?.Destroy();
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

        if (line != null)
        {
            foreach (VisualBubble bubble in trackedBubbles)
            {
                if (bubble.visual != null)
                {
                    DataPoint closestPoint = Utils.GetClosestPointOnLine(line.Start(), line.End(), bubble.Position);
                    bool isHit = Utils.IsLineTouchingCircle(line.Start(), line.End(), bubble.Position, .4f);
                    if (closestPoint.IsRealNumber())
                    {
                        VisualLine visual = lineManager.CreateLine(bubble.Position, closestPoint, isHit ? Color.green : Color.red, .02);
                        debugLines.Add(visual);
                    }
                }
            }
        }
    }
}
