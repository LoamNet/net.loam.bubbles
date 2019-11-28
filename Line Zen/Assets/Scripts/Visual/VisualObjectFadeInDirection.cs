using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class VisualObjectFadeInDirection : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    public float fadeTime = 1f;
    public float delayBeforeReset = .5f;
    public float speed = 3;

    private float timeSoFar = 0;
    private Vector3 startPos = Vector3.zero;

    private void Start()
    {
        timeSoFar = fadeTime;
        startPos = this.gameObject.transform.position;
        SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
        Color color = renderer.color;
        color.a = 0;
        renderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSoFar < fadeTime)
        {
            this.transform.position += direction * Time.deltaTime * speed;

            SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
            Color color = renderer.color;
            color.a = 1 - timeSoFar / fadeTime;
            renderer.color = color;
        }
        else if (timeSoFar > delayBeforeReset + fadeTime)
        {
            this.gameObject.transform.position = startPos;
            SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
            Color color = renderer.color;
            color.a = 1;
            renderer.color = color;

            timeSoFar = 0;
        }

        timeSoFar += Time.deltaTime;
    }
}
