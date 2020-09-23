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
            util.SaveToFile();
        }

        if (GUILayout.Button("Clear All"))
        {
            util.Clear();
        }

        if (GUILayout.Button("Copy to Clipboard"))
        {
            util.SaveToClipboard();
        }

        DrawDefaultInspector();
    }
}

#endif

