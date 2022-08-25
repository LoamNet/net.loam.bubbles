using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using UnityEngine;

// Custom editor for levels to make creating them easier
public class GameEditorUtility : MonoBehaviour
{
    public const int SILVER_DEFAULT = 25;
    public const int GOLD_DEFAULT = 55;
    public string fileName = "challenge0000";
    public string levelTitle = "Untitled";
    public int silver = SILVER_DEFAULT;
    public int gold = GOLD_DEFAULT;
    public List<GameObject> bubbles = new List<GameObject>();
    public List<GameObject> bubblesLarge = new List<GameObject>();


    [Header("Links")]
    public GameObject menu; 
    public Camera sceneCam;
    public GameObject templateRegular;
    public GameObject templateLarge;
    public GameInputManager inputManager;
    public SpriteRenderer safeArea;


    public bool MenuShowing { get { return menu.activeInHierarchy; } }

    public void Awake()
    {
        templateRegular.SetActive(false);
        templateLarge.SetActive(false);
        Clear();
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
            Destroy(bubbles[i]);
        }
        for (int i = 0; i < bubblesLarge.Count; ++i)
        {
            Destroy(bubblesLarge[i]);
        }

        bubbles.Clear();
        bubblesLarge.Clear();
    }

    public string Serialize()
    {
        const string end = "\n";
        const string sep = ",";

        string toWrite = "";
        toWrite += "name=" + levelTitle + end;
        toWrite += "stars=" + silver + sep + gold + end;

        for(int i = 0; i < bubbles.Count; ++i)
        {
            GameObject current = bubbles[i];
            float x = current.transform.position.x;
            float y = current.transform.position.y;
            toWrite += "bubble=" + x.ToString("n3") + sep + y.ToString("n3") + end;
        }

        for (int i = 0; i < bubblesLarge.Count; ++i)
        {
            GameObject current = bubblesLarge[i];
            float x = current.transform.position.x;
            float y = current.transform.position.y;
            toWrite += "bubbleLarge=" + x.ToString("n3") + sep + y.ToString("n3") + end;
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

    private void UpdateBubblePlacement()
    {
        // Handle inputs
        bool primary = inputManager.PrimaryInputPressed();
        bool secondary = inputManager.SecondaryInputPressed();

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
                    bubbles.Remove(hit.collider.gameObject);
                    bubblesLarge.Remove(hit.collider.gameObject);
                    Destroy(hit.collider.gameObject);
                }
            }

            // Verify we're in the safezone. If we are and nothing was hit, 
            // well, we can go ahead and place a bubble now!
            if (!hitSomething)
            {
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

                if (secondary)
                {
                    bubblesLarge.Add(newBubble);
                }
                else if (primary)
                {
                    bubbles.Add(newBubble);
                }
            }
        }
    }

}
