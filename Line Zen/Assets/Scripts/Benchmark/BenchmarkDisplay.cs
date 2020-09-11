using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchmarkDisplay : MonoBehaviour
{
    public BenchmarkLine entryTemplate;
    private List<BenchmarkLine> lines;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        lines = new List<BenchmarkLine>();
        entryTemplate.gameObject.SetActive(false);
        parent = entryTemplate.transform.parent;
    }

    public void AddItem(string nameText, string note)
    {
        BenchmarkLine line = GameObject.Instantiate(entryTemplate, parent);
        line.nameText.text = nameText;
        line.noteText.text = note;
        line.gameObject.SetActive(true);

        lines.Add(line);
    }
}
