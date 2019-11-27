using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIExpandIn : MonoBehaviour
{
    public float initialDelay = 1f;
    public float fadeTime = 2f;
    private float TimeSoFar = 0;

    private void Start()
    {
        this.transform.localScale = this.transform.localScale = new Vector3(1, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeSoFar - initialDelay < fadeTime)
        {
            TimeSoFar += Time.deltaTime;

            if (TimeSoFar < initialDelay)
                return;

            float scale = (TimeSoFar - initialDelay) / fadeTime;
            this.transform.localScale = new Vector3(1, scale, 1);

            if (TimeSoFar - initialDelay >= fadeTime)
            {
                this.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
