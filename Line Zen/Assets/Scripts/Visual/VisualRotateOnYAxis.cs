using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualRotateOnYAxis : MonoBehaviour
{
    public float rotateAmount = 3f;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0, 1, 0), rotateAmount * Time.deltaTime);
    }
}
