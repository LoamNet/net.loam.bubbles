using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TMPro.TextMeshProUGUI ))]
public class FPS : MonoBehaviour
{
    public static float CurrentFPS { get; private set; }

    const float fpsMeasurePeriod = 1f;
    private float fpsAccumulator = 0;
    private float fpsNextPeriod = 0;
    private TMPro.TextMeshProUGUI display;


    private void Start()
    {
        fpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
        display = GetComponent<TMPro.TextMeshProUGUI>();
    }


    private void Update()
    {
        // measure average frames per second
        fpsAccumulator++;
        if (Time.realtimeSinceStartup > fpsNextPeriod)
        {
            CurrentFPS = (fpsAccumulator / fpsMeasurePeriod);
            fpsAccumulator = 0;
            fpsNextPeriod += fpsMeasurePeriod;
            display.text = string.Format("{0:0}", CurrentFPS);
        }
    }
}
