using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILevelEditorWriter : MonoBehaviour
{
    public GameEditorUtility editor;
    public TMPro.TMP_InputField fileName;
    public TMPro.TMP_InputField levelName;
    public TMPro.TMP_InputField starBronze;
    public TMPro.TMP_InputField starSilver;
    public TMPro.TMP_InputField starGold;
    public Button clearAll;
    public Button saveAll;

    [Header("Internal Tracked Values")]
    [SerializeField] private string filename;

    // Start is called before the first frame update
    void Start()
    {
        // String value parsing
        fileName.onValueChanged.AddListener((newval) => { filename = newval; });
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
        clearAll.onClick.AddListener(() => { editor.Clear(); });
    }

    void UpdateToEditorContent()
    {
        levelName.text = editor.levelTitle;
        starBronze.text = editor.bronze.ToString();
        starSilver.text = editor.silver.ToString();
        starBronze.text = editor.gold.ToString();
    }
}
