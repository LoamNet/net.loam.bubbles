﻿using System.Collections;
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
    public TextMeshProUGUI scoreText;
    public CanvasGroup scoreTextArea;

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
    [Space]
    public Image levelCategoryIcon;
    [Header("Index 0 corresponds to group 0")]
    public List<Sprite> groupEntries;

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
    public void Initialize(string file, int stars, string title, long score, int group = -1)
    {
        this.file = file;

        if(group >= 0)
        {
            // shift by one
            levelCategoryIcon.sprite = groupEntries[group];
        }

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


        if(this.scoreText != null)
        {
            if (score > 0)
            {
                if (this.scoreTextArea != null)
                {
                    this.scoreTextArea.alpha = 1;
                }
                this.scoreText.text = score.ToString();
            }
            else
            {
                if (this.scoreTextArea != null)
                {
                    this.scoreTextArea.alpha = 0;
                }
                this.scoreText.text = "";
            }
        }
        else
        {
            if(this.scoreTextArea != null)
            {
                this.scoreTextArea.alpha = 0;
            }
        }
    }
}
