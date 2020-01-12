using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIFIleDialogBase : MonoBehaviour
{
    public Button cancel;
    public Button select;
    public Button directoryUp;
    public TMPro.TextMeshProUGUI textPath;
    public GameObject entryTemplate;
    public Sprite fileIcon;
    public Sprite folderIcon;
    public UIFadeOut errorSymbol;

    private List<GameObject> entries;
    private Transform entryParent;
    private string _currentPath;
    public string CurrentPath
    {
        get
        {
            return _currentPath;
        }
        set
        {
            UpdateDialogWithDirectoryInfo(value);
        }
    }

    // Collect some basic info and hide the template in the dialog.
    void Start()
    {
        entries = new List<GameObject>();
        entryTemplate.SetActive(false);
        entryParent = entryTemplate.gameObject.transform.parent;

        // Bind buttons
        directoryUp.onClick.AddListener(() => {
            int index = CurrentPath.LastIndexOf(System.IO.Path.DirectorySeparatorChar);

            if(index <= 0)
            {
                index = CurrentPath.LastIndexOf(System.IO.Path.AltDirectorySeparatorChar);
            }

            if (index > 0)
            {
                CurrentPath = CurrentPath.Substring(0, index);
            }
        });

        // Sets the current path and adjusts accordingly
        CurrentPath = Application.persistentDataPath;
    }
 
    // Update is called once per frame
    void UpdateDialogWithDirectoryInfo(string newValue)
    {
        DirectoryInfo d = null;
        FileInfo[] files = null;
        DirectoryInfo[] directories = null;

        // Start by getting the path and files and seeing if we can proceed.
        // If we don't end with an acceptable directory separator character, add the default one.
        string adjustedPath = newValue.Trim();
        if (!adjustedPath.EndsWith("" + Path.DirectorySeparatorChar) && !adjustedPath.EndsWith("" + Path.AltDirectorySeparatorChar))
        {
            adjustedPath += Path.DirectorySeparatorChar;
        }

        // Attempt to get directory and file info. If we error out, report that to the user
        // but allow them to continue to use things, so exit early to avoid clearing out folders/etc.
        try
        {
            d = new DirectoryInfo(adjustedPath);
            files = d.GetFiles();
            directories = d.GetDirectories();
        }
        catch(System.Exception)
        {
            errorSymbol.ResetAlpha();
            return;
        }

        // Now, we can finally keep tabs since we've confirmed this is updated.
        _currentPath = newValue.Trim();

        // Clear old content and update path string
        foreach (GameObject g in entries)
        {
            Destroy(g);
        }
        entries.Clear();
        textPath.text = _currentPath;

        // Display all folders
        foreach (DirectoryInfo dir in directories)
        {
            AddListItem(dir.Name, folderIcon, true);
        }

        // Display all files
        foreach (FileInfo file in files)
        {
            AddListItem(file.Name, fileIcon, false);
        }
    }

    // Creates a list item and generates the appropriate callbacks for interacting with the buttons
    private void AddListItem(string name, Sprite icon, bool selectable)
    {
        GameObject newLine = GameObject.Instantiate(entryTemplate, entryParent);
        newLine.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = name;
        newLine.SetActive(true);

        foreach (Image img in newLine.GetComponentsInChildren<Image>())
        {
            if (img.gameObject.name.ToLowerInvariant().Equals("icon"))
            {
                img.sprite = icon;
            }
        }

        if(selectable)
        {
            newLine.GetComponent<Button>().onClick.AddListener(() => {
                CurrentPath = CurrentPath + System.IO.Path.DirectorySeparatorChar + name;
            });
        }
        else
        {
            newLine.GetComponent<Button>().interactable = false;
        }
        entries.Add(newLine);
    }
}
