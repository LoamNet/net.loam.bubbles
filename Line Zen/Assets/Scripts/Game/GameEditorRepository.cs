using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Slightly cursed data store
/// </summary>
public class GameEditorRepository : MonoBehaviour
{
    /// <summary>
    /// Current level, if present, from the editor.
    /// </summary>
    [System.NonSerialized] public string LevelDataFromEditor = null;
    [System.NonSerialized] public string LevelDataFromRunner = null;
    [System.NonSerialized] public static GameEditorRepository Instance = null;

    private void Start()
    {
        // Prevent duplicates
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ...
    }
}
