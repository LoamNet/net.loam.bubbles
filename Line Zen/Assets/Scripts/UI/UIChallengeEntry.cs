using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UIChallengeEntry : MonoBehaviour
{
    public Events events;

    [Header("Info")]
    public TextMeshProUGUI title;

    [Header("Stars")]
    public Sprite unstarred;
    public Color unstarredColor;
    [Space]
    public Sprite starred;
    public Color starredColor;
    [Space]
    public Image star1;
    public Image star2;
    public Image star3;

    private string file;

    public void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            events.OnLevelLoadRequest?.Invoke(file);
        });
    }

    // Set default values and content, and adjust display settings
    public void Initialize(string file, int stars, string title)
    {
        this.file = file;

        if (stars >= 3)
        {
            star3.sprite = starred;
            star3.color = starredColor;
        }
        else
        {
            star3.sprite = unstarred;
            star3.color = unstarredColor;
        }

        if (stars >= 2)
        {
            star2.sprite = starred;
            star2.color = starredColor;
        }
        else
        {
            star2.sprite = unstarred;
            star2.color = unstarredColor;
        }

        if (stars >= 1)
        {
            star1.sprite = starred;
            star1.color = starredColor;
        }
        else
        {
            star1.sprite = unstarred;
            star1.color = unstarredColor;
        }

        if (this.title != null)
        {
            this.title.text = title;
        }
    }
}
