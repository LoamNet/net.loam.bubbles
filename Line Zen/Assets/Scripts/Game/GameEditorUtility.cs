using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

// Custom editor for levels to make creating them easier
public class GameEditorUtility : MonoBehaviour
{
    public class BubbleEntry : System.IDisposable
    {
        public GameObject bubble;
        public DataPoint movementOffset;
        private LineRenderer bundledLine;

        public BubbleEntry(GameObject bubble)
        {
            this.bubble = bubble;
        }

        public void BundleLine(LineRenderer linePrefab, DataPoint start, DataPoint end)
        {
            Color color = Color.grey;
            bundledLine = Instantiate(linePrefab);
            bundledLine.enabled = true;
            bundledLine.startColor = color;
            bundledLine.endColor = color;
            GameEditorUtility.SetLine(bundledLine, start, end);
        }

        public int DetermineSpeed()
        {
            int speed = Mathf.RoundToInt(((Vector2)movementOffset).magnitude);
            if(speed > 0)
            {
                speed += MIN_MOVE_TIME;
                speed *= MOVE_SCALAR;
            }

            return speed;
        }

        public void Dispose()
        {
            Destroy(bubble);

            if (bundledLine != null)
            {
                Destroy(bundledLine.gameObject);
            }
        }
    }

    public const int MIN_MOVE_TIME = 1;
    public const int MOVE_SCALAR = 2;
    public const int SILVER_DEFAULT = 25;
    public const int GOLD_DEFAULT = 55;
    public string fileName = "challenge0000";
    public string levelTitle = "Untitled";
    public int silver = SILVER_DEFAULT;
    public int gold = GOLD_DEFAULT;
    public List<BubbleEntry> bubbles = new List<BubbleEntry>();
    public List<BubbleEntry> bubblesLarge = new List<BubbleEntry>();


    [Header("Links")]
    public LineRenderer dragLine;
    public GameObject menu; 
    public Camera sceneCam;
    public GameObject templateRegular;
    public GameObject templateLarge;
    public GameInputManager inputManager;
    public SpriteRenderer safeArea;

    private bool previousPrimary = false;
    private bool previousSecondary = false;
    private bool draggingPrimary = false;
    private bool draggingSecondary = false;
    private DataPoint dragStartPos = new DataPoint();
    private BubbleEntry lastPlaced = null;

    public bool MenuShowing { get { return menu.activeInHierarchy; } }

    public void Awake()
    {
        templateRegular.SetActive(false);
        templateLarge.SetActive(false);
        Clear();
        dragLine.enabled = false;
    }

    public void SaveToFile()
    {
#if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.SaveFilePanel("Save level",
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
            fileName,
            "txt");

        if (!string.IsNullOrEmpty(path))
        {
            File.WriteAllText(path, this.Serialize());
        }
#else
        Debug.LogError("There is no path here");
#endif

    }

    public void SaveToClipboard(bool compressed = false)
    {
        string toClipboard;

        if (compressed)
        {
            string toWrite = this.Serialize();
            string nowCompressed = Utils.CompressString(toWrite);
            toClipboard = $"{GameCore.compressionIndicator}{nowCompressed}";
        }
        else
        {
            toClipboard = this.Serialize();
        }

        GUIUtility.systemCopyBuffer = toClipboard;
    }

    public void Clear()
    {
        fileName = "challenge0000";
        levelTitle = "Untitled";
        silver = SILVER_DEFAULT;
        gold = GOLD_DEFAULT;

        for (int i = 0; i < bubbles.Count; ++i)
        {
            bubbles[i].Dispose();
        }
        for (int i = 0; i < bubblesLarge.Count; ++i)
        {
            bubblesLarge[i].Dispose();
        }

        bubbles.Clear();
        bubblesLarge.Clear();
    }

    const string keyValSep = "=";
    const string sep = ",";
    const string sepSplit = ":";
    const string sepSpeed = "@";
    const string end = "\n";

    private string Format(string key, BubbleEntry value)
    {
        string decimalFormatString = "n2";

        string x = value.bubble.transform.position.x.ToString(decimalFormatString);
        string y = value.bubble.transform.position.y.ToString(decimalFormatString);
        string x_vel = value.movementOffset.X.ToString(decimalFormatString);
        string y_vel = value.movementOffset.Y.ToString(decimalFormatString);
        string speed = value.DetermineSpeed().ToString();

        return key + keyValSep + x + sep + y + sepSplit + x_vel + sep + y_vel + sepSpeed + speed + end;
    }

    public string Serialize()
    {
        string toWrite = "";
        toWrite += "name" + keyValSep + levelTitle + end;
        toWrite += "stars" + keyValSep + silver + sep + gold + end;

        for(int i = 0; i < bubbles.Count; ++i)
        {
            BubbleEntry current = bubbles[i];
            toWrite += Format("bubbles", current);
        }

        for (int i = 0; i < bubblesLarge.Count; ++i)
        {
            BubbleEntry current = bubblesLarge[i];
            toWrite += Format("bubbleLarge", current);
        }

        return toWrite;
    }

    private void Update()
    {
        // Check for copying out
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            if (Input.GetKey(KeyCode.C))
            {
                SaveToClipboard(compressed: false);
            }
        }

        // Process inputs if we're not locked by the menu
        if (!MenuShowing)
        {
            UpdateBubblePlacement();
        }
    }

    /// <summary>
    /// Given a target list of bubble entries and something to remove, search 
    /// for and remove ONLY the FIRST INSTANCE from the front ([0]) to back (Count) of the list.
    /// Return if we did or didn't find the item and remove it.
    /// </summary>
    private bool RemoveEntry(List<BubbleEntry> targetList, GameObject toRemove)
    {
        for(int i = 0; i < targetList.Count; ++i)
        {
            BubbleEntry entry = targetList[i];

            if (entry.bubble == toRemove)
            {
                targetList[i].Dispose();
                targetList.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public static void SetLine(LineRenderer line, DataPoint start, DataPoint end)
    {
        line.SetPosition(0, new Vector3(start.X, start.Y, 0));
        line.SetPosition(1, new Vector3(end.X, end.Y));
    }

    private void SetPreviewLine(DataPoint start, DataPoint end)
    {
        SetLine(dragLine, start, end);
    }

    private void UpdateBubblePlacement()
    {
        // Handle inputs
        bool primaryDown = inputManager.PrimaryInputDown(); // always true if down
        bool secondaryDown = inputManager.SecondaryInputDown(); // always true if down
        bool primary = inputManager.PrimaryInputPressed(); // only true on first frame
        bool secondary = inputManager.SecondaryInputPressed(); // only true on first frame
        bool finishedDrag = false;

        // See if we're starting a drag
        if((primaryDown && previousPrimary == false) || (secondaryDown && previousSecondary == false))
        {
            if(primaryDown && previousPrimary == false)
            {
                draggingPrimary = true;
            }
            if(secondaryDown && previousSecondary == false)
            {
                draggingSecondary = true;
            }

            dragStartPos = inputManager.PrimaryInputPosWorld();
            dragLine.enabled = true;
            SetPreviewLine(dragStartPos, dragStartPos); // intentionally both the same point so the line is active but not visible.
        }

        // See if we stopped drag
        if(draggingPrimary && primaryDown == false)
        {
            finishedDrag = true;
            draggingPrimary = false;
        }

        if(draggingSecondary && secondaryDown == false)
        {
            finishedDrag = true;
            draggingSecondary = false;
        }

        // Update preview if dragging
        if(draggingPrimary || draggingSecondary)
        {
            SetPreviewLine(dragStartPos, inputManager.PrimaryInputPosWorld());
        }

        // Once we finished dragging the difference (input up)
        if(finishedDrag)
        {
            DataPoint endPos = inputManager.PrimaryInputPosWorld();
            DataPoint difference = endPos - dragStartPos;

            if (lastPlaced != null)
            {
                float differenceLen = Mathf.RoundToInt(((Vector2)difference).magnitude);
                if (differenceLen > GameCore.bubbleRadiusStandard)
                {
                    lastPlaced.movementOffset = difference;
                    lastPlaced.BundleLine(dragLine, dragStartPos, endPos);
                }
            }

            // Return to defaults
            lastPlaced = null;
            dragStartPos = new DataPoint();
            dragLine.enabled = false;
        }

        // Place something. (input pressed, one frame only)
        if (primary || secondary)
        {
            RaycastHit hit;
            bool hitSomething = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Cast a ray into the scene and see if we hit anything.
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    hitSomething = true;
                    GameObject toTryToRemove = hit.collider.gameObject;

                    // Try both lists. Will only be in one of them.
                    RemoveEntry(bubbles, toTryToRemove);
                    RemoveEntry(bubblesLarge, toTryToRemove);
                }
            }

            // Early exit if we handled hitting existing things
            if(hitSomething)
            {
                lastPlaced = null;
                return;
            }

            // Verify we're in the safezone. If we are and nothing was hit, 
            // well, we can go ahead and place a bubble now!
            DataPoint pos = inputManager.PrimaryInputPosWorld();

            float left = safeArea.transform.position.x - safeArea.size.x / 2;
            float right = safeArea.transform.position.x + safeArea.size.x / 2;
            float top = safeArea.transform.position.y + safeArea.size.y / 2;
            float bottom = safeArea.transform.position.y - safeArea.size.y / 2;

            if (pos.X > right || pos.X < left || pos.Y > top || pos.Y < bottom)
            {
                return;
            }

            // Default to regular bubble
            GameObject toSpawn = templateRegular;
            float radius = GameCore.bubbleRadiusStandard;

            // Override to large bubble
            if (secondary)
            {
                toSpawn = templateLarge;
                radius = GameCore.bubbleRadiusLarge;
            }

            // Determine scale accordingly
            radius /= toSpawn.transform.localScale.x;

            // Spawn the bubble, and provide the collider.
            GameObject newBubble = GameObject.Instantiate(toSpawn);
            SphereCollider col = newBubble.AddComponent<SphereCollider>();
            newBubble.transform.position = pos;
            newBubble.SetActive(true);
            col.radius = radius;

            BubbleEntry entry = new BubbleEntry(newBubble);
            if (secondary)
            {
                bubblesLarge.Add(entry);
            
            }
            else if (primary)
            {
                bubbles.Add(entry);
            }
            lastPlaced = entry;
        }

        previousPrimary = primaryDown;
        previousSecondary = secondaryDown;
    }

}
