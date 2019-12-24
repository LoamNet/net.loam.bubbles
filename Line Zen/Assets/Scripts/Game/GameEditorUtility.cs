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
    public List<GameObject> spawned = new List<GameObject>();

    [Header("Links")]
    public Camera sceneCam;
    public GameObject template;
    public GameInputManager inputManager;
    public SpriteRenderer safeArea;

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
#endif

    }

    public void Clear()
    {
        fileName = "challenge0000";
        levelTitle = "Untitled";
        bronze = 3;
        silver = 2;
        gold = 1;

        for (int i = 0; i < spawned.Count; ++i)
        {
            Destroy(spawned[i]);
        }

        spawned.Clear();
    }

    public string Serialize()
    {
        const string end = "\n";
        const string sep = ",";

        string toWrite = "";
        toWrite += "name=" + levelTitle + end;
        toWrite += "stars=" + bronze + sep + silver + sep + gold + end;

        for(int i = 0; i < spawned.Count; ++i)
        {
            GameObject current = spawned[i];
            float x = current.transform.position.x;
            float y = current.transform.position.y;
            toWrite += "bubble=" + x.ToString("n3") + sep + y.ToString("n3") + end;
        }

        return toWrite;
    }

    private void Update()
    {
        if(inputManager.PrimaryInputPressed())
        {
            RaycastHit hit;
            bool hitSomething = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null)
                {
                    hitSomething = true;
                    spawned.Remove(hit.collider.gameObject);
                    Destroy(hit.collider.gameObject);
                }
            }

            if (!hitSomething)
            {
                DataPoint pos = inputManager.PrimaryInputPosWorld();

                float left = safeArea.transform.position.x - safeArea.size.x / 2;
                float right = safeArea.transform.position.x + safeArea.size.x / 2;
                float top = safeArea.transform.position.y + safeArea.size.y / 2;
                float bottom = safeArea.transform.position.y - safeArea.size.x / 2;

                if (pos.X > right || pos.X < left)
                    return;

                if (pos.Y > top || pos.Y < bottom)
                    return;

                GameObject newBubble = GameObject.Instantiate(template);
                newBubble.transform.position = pos;
                newBubble.SetActive(true);
                SphereCollider col = newBubble.AddComponent<SphereCollider>();
                col.radius = .5f;
                spawned.Add(newBubble);
            }
        }
    }
}
