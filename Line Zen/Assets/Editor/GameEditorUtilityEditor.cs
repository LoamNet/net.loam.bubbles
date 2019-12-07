using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR

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
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop),
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

