using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIChallengeEntry : MonoBehaviour
{
    [Header("Info")]
    public TextMeshProUGUI title;

    [Header("Stars")]
    public Sprite unstarred;
    public Sprite starred;
    public Image star1;
    public Image star2;
    public Image star3;

    public void Initialize(int stars, string title)
    {
        if (stars >= 3) { star3.sprite = starred; } else { star3.sprite = unstarred; }
        if (stars >= 2) { star2.sprite = starred; } else { star2.sprite = unstarred; }
        if (stars >= 1) { star1.sprite = starred; } else { star1.sprite = unstarred; }

        this.title.text = title;
    }
}
