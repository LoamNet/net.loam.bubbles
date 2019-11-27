using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TextType
{
    ScoreAddition,
    ScoreAdditionBonus
}

public class VisualScoreTextFeedback : MonoBehaviour
{
    [Header("General")]
    public Events events;
    public TMPro.TextMeshProUGUI textScoreVisual;
    public Canvas canvastarget;

    [Header("Text Colors")]
    public Color scoreAddition;
    public Color scoreAdditionBonus;

    // Private variables
    private List<TMPro.TextMeshProUGUI> createdText;

    private void Start()
    {
        createdText = new List<TMPro.TextMeshProUGUI>();
    }

    public void CreateText(DataPoint position, string value, TextType textType)
    {
        TMPro.TextMeshProUGUI visual = Instantiate(textScoreVisual);

        visual.transform.SetParent(canvastarget.transform);
        visual.transform.position = position;
        visual.text = value;

        switch(textType)
        {
            case TextType.ScoreAddition:
                visual.color = scoreAddition;
                break;

            case TextType.ScoreAdditionBonus:
                visual.color = scoreAdditionBonus;
                break;
        }

        createdText.Add(visual);
    }

    // Update is called once per frame
    void Update()
    {
        foreach(TMPro.TextMeshProUGUI textVisual in createdText)
        {
            Vector3 pos = textVisual.transform.position;
            textVisual.transform.position = new Vector3(pos.x, pos.y + 10f * Time.deltaTime, pos.z);

            Color col = textVisual.color;
            textVisual.color = new Color(col.r, col.g, col.b, col.a - .01f);
        }
    }
}
