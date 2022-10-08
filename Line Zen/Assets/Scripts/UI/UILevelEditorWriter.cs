using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelEditorWriter : MonoBehaviour
{
    public GameEditorUtility editor;
    public InputField levelName;
    public InputField star3;
    public InputField star2;
    public Button clearAll;
    public Button saveAll;
    public Button toClipboard;

    // Start is called before the first frame update
    private void OnEnable()
    {
        // String value parsing
        levelName.onValueChanged.AddListener(LevelNameChanged);

        star3.onValueChanged.AddListener(SetGoldValue);
        star2.onValueChanged.AddListener(SetSilverValue);

        // Button hooking up
        clearAll.onClick.AddListener(() => { editor.Clear(); UpdateToEditorContent(); });
        saveAll.onClick.AddListener(() => { editor.SaveToFile(); UpdateToEditorContent(); });
        toClipboard.onClick.AddListener(() => { editor.SaveToClipboard(compressed: false); });

        // Initial reset
        UpdateToEditorContent();
    }

    private void OnDisable()
    {
        levelName.onValueChanged.RemoveListener(LevelNameChanged);
        star3.onValueChanged.RemoveListener(SetGoldValue);
        star2.onValueChanged.RemoveListener(SetSilverValue);
        clearAll.onClick.RemoveAllListeners();
        saveAll.onClick.RemoveAllListeners();
    }

    void LevelNameChanged(string newval)
    {
        editor.levelTitle = newval;
    }


    void SetGoldValue(string newval)
    {
        if (int.TryParse(newval, out int result))
        {
            editor.gold = result;
        }
        else
        {
            star3.text = "";
        }
    }

    void SetSilverValue(string newval)
    {
        if (int.TryParse(newval, out int result))
        {
            editor.silver = result;
        }
        else
        {
            star2.text = "";
        }
    }


    // This sets the state of the UI to the state of the editor internally.
    public void UpdateToEditorContent()
    {
        if (levelName.text != editor.levelTitle)
        {
            levelName.text = editor.levelTitle;
        }

        string silver = editor.silver.ToString();
        if (star2.text != silver)
        {
            star2.text = silver;
        }

        string gold = editor.gold.ToString();
        if (star3.text != gold)
        {
            star3.text = gold;
        }
    }
}
