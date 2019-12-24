using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelEditorWriter : MonoBehaviour
{
    public GameEditorUtility editor;
    public InputField fileName;
    public InputField levelName;
    public InputField starBronze;
    public InputField starSilver;
    public InputField starGold;
    public Button clearAll;
    public Button saveAll;

    // Start is called before the first frame update
    void Start()
    {
        // String value parsing
        fileName.onValueChanged.AddListener((newval) => { editor.fileName = newval; });
        levelName.onValueChanged.AddListener((newval) => { editor.levelTitle = newval; });

        // Star parsing/binding
        starBronze.onValueChanged.AddListener((newval) => {
            if (int.TryParse(newval, out int result))
            {
                editor.bronze = result;
            }
        });
        starSilver.onValueChanged.AddListener((newval) => {
            if (int.TryParse(newval, out int result))
            {
                editor.silver = result;
            }
        });
        starGold.onValueChanged.AddListener((newval) => {
            if (int.TryParse(newval, out int result))
            {
                editor.gold = result;
            }
        });

        // Button hooking up
        clearAll.onClick.AddListener(() => { editor.Clear(); UpdateToEditorContent(); });

        // Initial reset
        UpdateToEditorContent();
    }

    // This sets the state of the UI to the state of the editor internally.
    void UpdateToEditorContent()
    {
        levelName.text = editor.levelTitle;
        fileName.text = editor.fileName;
        starBronze.text = editor.bronze.ToString();
        starSilver.text = editor.silver.ToString();
        starGold.text = editor.gold.ToString();
    }
}
