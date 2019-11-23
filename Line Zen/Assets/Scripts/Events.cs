using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events : MonoBehaviour
{
    // When a line is created.
    public Action<DataPoint, DataPoint> OnLineCreated;
    
    // This is the action performed when a line is drawn, and contains a start and end accordingly.
    // It's more of an upate.
    public Action<DataPoint, DataPoint> OnLineUpdated;

    // When a line is termianted.
    public Action OnLineDestroyed;

    // When a tutorial swipe is done
    public Action OnCorrectTutorialSwipe;

    // Whenever the state of bubbles changes, this is fired.
    public Action<List<DataPoint>> OnBubblesChange;
}
