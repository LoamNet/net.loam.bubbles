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

        if (events != null)
        {
            events.OnGameStateChangeRequest += StartShrinkOut;
        }
        else
        {
            name = $"[No Events] {name}";
        }

        timeout = Random.Range(2f, 5f);
        size = GameCore.bubbleRadiusStandard * 2;
    }

    private void OnDestroy()
    {
        if (events != null)
        {
            events.OnGameStateChangeRequest -= StartShrinkOut;
        }
    }

    // Handle what states the bubbles pop themselves.
    private void StartShrinkOut(GameState state, GameMode mode)
    {
        if(state == GameState.Startup || state == GameState.PickChallenge || state == GameState.Extras)
        {
            isShrinking = true;
            return;
        }

        return;
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

                if (events != null)
                {
                    events.OnBubbleDestroyed?.Invoke(new DataPoint(gameObject.transform.position));
                }

                Destroy(this.gameObject);
            }
        }
    }
}
