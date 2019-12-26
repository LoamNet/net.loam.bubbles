using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UIFIleDialogBase : MonoBehaviour
{
    public Button cancel;
    public Button select;
    public TMPro.TextMeshProUGUI textPath;
    public GameObject entryTemplate;

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
        entryTemplate.SetActive(false);
        entryParent = entryTemplate.gameObject.transform.parent;
        CurrentPath = Application.dataPath;
    }

    // Update is called once per frame
    void UpdateDialogWithDirectoryInfo()
    {
        textPath.text = _currentPath;
        DirectoryInfo d = new DirectoryInfo(_currentPath);
        FileInfo[] files = d.GetFiles();
        
        foreach(GameObject g in entries)
        {
            Destroy(g);
        }
        entries.Clear();

        foreach (FileInfo file in files)
        {
            GameObject newLine = GameObject.Instantiate(entryTemplate, entryParent);
            newLine.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = file.Name;
            newLine.SetActive(true);

            entries.Add(newLine);
        }
    }
}
