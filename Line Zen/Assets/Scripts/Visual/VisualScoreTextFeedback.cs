using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum TextType
{
    ScoreAddition,
    ScoreAdditionBonus
}

public class VisualScoreTextFeedback : MonoBehaviour
{
    [Header("General")]
    public Events events;
    public TextMeshProUGUI textScoreVisual;
    public Canvas canvastarget;

    [Header("Text Colors")]
    public Color scoreAddition;
    public Color scoreAdditionBonus;

    // Private variables
    private List<TextMeshProUGUI> createdText;

    private void Start()
    {
        createdText = new List<TMPro.TextMeshProUGUI>();
    }

    public void CreateText(DataPoint position, string value, TextType textType)
    {
        TextMeshProUGUI visual = Instantiate(textScoreVisual, canvastarget.transform);
        visual.transform.position = position;
        visual.text = value;


        switch (textType)
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
        List<TextMeshProUGUI> expired = new List<TextMeshProUGUI>();

        foreach(TextMeshProUGUI textVisual in createdText)
        {
            Vector3 pos = textVisual.transform.position;
            textVisual.transform.position = new Vector3(pos.x, pos.y + 10f * Time.deltaTime, pos.z);

            Color col = textVisual.color;
            textVisual.color = new Color(col.r, col.g, col.b, col.a - .5f * Time.deltaTime);

            if (textVisual.color.a < 0.01)
            {
                expired.Add(textVisual);
            }
        }

        foreach(TextMeshProUGUI old in expired)
        {
            createdText.Remove(old);
            Destroy(old.gameObject);
        }
    }
}
