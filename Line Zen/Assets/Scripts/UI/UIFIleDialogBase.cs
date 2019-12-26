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
            _currentPath = value;
            UpdateDialogWithDirectoryInfo();
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
        CurrentPath = Application.dataPath;
    }

    // Update is called once per frame
    void UpdateDialogWithDirectoryInfo()
    {
        // Clear old content and update path string
        foreach (GameObject g in entries)
        {
            Destroy(g);
        }
        entries.Clear();
        textPath.text = _currentPath;

        // Get path. If we don't end with an acceptable directory separator character, add the default one.
        string adjustedPath = _currentPath.Trim();
        if(!adjustedPath.EndsWith("" + Path.DirectorySeparatorChar) && !adjustedPath.EndsWith("" + Path.AltDirectorySeparatorChar))
        {
            adjustedPath += Path.DirectorySeparatorChar;
        }
        
        DirectoryInfo d = new DirectoryInfo(adjustedPath);
        FileInfo[] files = d.GetFiles();
        DirectoryInfo[] directories = d.GetDirectories();

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
