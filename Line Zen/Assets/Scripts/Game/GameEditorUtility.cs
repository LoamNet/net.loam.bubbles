using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// Custom editor for levels to make creating them easier
public class GameEditorUtility : MonoBehaviour
{
    public string fileName = "challenge0000";
    public string levelTitle = "Untitled";
    public int bronze = 3;
    public int silver = 2;
    public int gold = 1;
    public List<GameObject> bubbles = new List<GameObject>();
    public List<GameObject> bubblesLarge = new List<GameObject>();


    [Header("Links")]
    public Camera sceneCam;
    public GameObject templateRegular;
    public GameObject templateLarge;
    public GameInputManager inputManager;
    public SpriteRenderer safeArea;

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

    public void SaveToClipboard()
    {
        GUIUtility.systemCopyBuffer = this.Serialize();
    }

    public void Clear()
    {
        fileName = "challenge0000";
        levelTitle = "Untitled";
        bronze = 3;
        silver = 2;
        gold = 1;

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
        toWrite += "stars=" + bronze + sep + silver + sep + gold + end;

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
                float radius = 1;

                // Override to large bubble
                if(secondary)
                {
                    toSpawn = templateLarge;
                    radius = 1.0f;
                }

                GameObject newBubble = GameObject.Instantiate(toSpawn);
                newBubble.transform.position = pos;
                newBubble.SetActive(true);

                SphereCollider col = newBubble.AddComponent<SphereCollider>();
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
