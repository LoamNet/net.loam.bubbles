using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

using System;
using System.IO;
using UnityEditor;

[CustomEditor(typeof(GameEditorUtility))]
public class GameEditorUtilityEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameEditorUtility util = (GameEditorUtility)this.target;
        if (GUILayout.Button("Save As"))
        {
            string path = EditorUtility.SaveFilePanel(
                "Save level", 
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                "challenge0000", 
                "txt");

            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, util.Serialize());
            }
        }

        if (GUILayout.Button("Clear All"))
        {
            util.Clear();
        }

        DrawDefaultInspector();
    }
}

#endif

// Custom editor for levels to make creating them easier
public class GameEditorUtility : MonoBehaviour
{
    public string levelTitle = "Untitled";
    public int bronze = 3;
    public int silver = 2;
    public int gold = 1;
    public List<GameObject> spawned = new List<GameObject>();

    [Header("Links")]
    public Camera sceneCam;
    public GameObject template;
    public GameInputManager inputManager;

    public void Clear()
    {
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
                GameObject newBubble = GameObject.Instantiate(template);
                newBubble.transform.position = inputManager.PrimaryInputPosWorld();
                newBubble.SetActive(true);
                SphereCollider col = newBubble.AddComponent<SphereCollider>();
                col.radius = .5f;
                spawned.Add(newBubble);
            }
        }
    }
}
