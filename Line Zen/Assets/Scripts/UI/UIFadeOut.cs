using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class UIFadeOut : MonoBehaviour
{
    public float initialDelay = 1f;
    public float fadeTime = 2f;
    public bool startHidden = false;

    private UnityEngine.UI.Image img;
    private float initialAlpha = 0;
    private float timeSoFar = 0;

    private void Start()
    {
        img = gameObject.GetComponent<UnityEngine.UI.Image>();
        initialAlpha = img.color.a;

        if(startHidden)
        {
            timeSoFar = float.MaxValue;
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        }
    }

    void Update()
    {
        if (timeSoFar - initialDelay < fadeTime)
        {
            timeSoFar += Time.deltaTime;

            if (timeSoFar < initialDelay)
            {
                return;
            }

            float alpha = initialAlpha - (timeSoFar - initialDelay) / fadeTime;
            img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);

            if (timeSoFar - initialDelay >= fadeTime)
            {
                img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            }
        }
    }

    public void ResetAlpha()
    {
        img.color = new Color(img.color.r, img.color.g, img.color.b, initialAlpha);
        timeSoFar = 0;
    }
}
