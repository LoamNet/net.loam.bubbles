using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualRemoveBubbleOnMenu : MonoBehaviour
{
    public Events events;
    private float timeout;
    private bool isShrinking;
    private float size;

    // Start is called before the first frame update
    void Start()
    {
        isShrinking = false;
        events.OnGameStateChangeRequest += StartShrinkOut;
        timeout = Random.Range(2f, 5f);
        size = GameCore.bubbleRadius * 2;
    }

    private void OnDestroy()
    {
        events.OnGameStateChangeRequest -= StartShrinkOut;
    }

    private void StartShrinkOut(GameState state, GameMode mode)
    {
        if(state != GameState.Startup)
        {
            return;
        }

        isShrinking = true;
    }

    private void Update()
    {
        if(isShrinking)
        {
            size -= Time.deltaTime * timeout;

            if (size > 0)
            {
                this.gameObject.transform.localScale = new Vector3(size, size, size);
            }
            else
            {
                isShrinking = false;
                events.OnBubbleDestroyed?.Invoke(new DataPoint(gameObject.transform.position));
                Destroy(this.gameObject);
            }
        }
    }
}
