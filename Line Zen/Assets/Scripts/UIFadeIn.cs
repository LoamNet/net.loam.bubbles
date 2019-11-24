using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UIFadeIn : MonoBehaviour
{
    public float initialDelay = 1f;
    public float fadeTime = 2f;
    private TMPro.TextMeshProUGUI textObj;
    private float TimeSoFar = 0;

    private void Start()
    {
        textObj = gameObject.GetComponent<TMPro.TextMeshProUGUI>();
        textObj.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(TimeSoFar - initialDelay < fadeTime)
        {
            TimeSoFar += Time.deltaTime;

            if (TimeSoFar < initialDelay)
                return;

            textObj.alpha = (TimeSoFar - initialDelay) / fadeTime;

            if(TimeSoFar - initialDelay >= fadeTime)
            {
                textObj.alpha = 1f;
            }
        }
    }
}
