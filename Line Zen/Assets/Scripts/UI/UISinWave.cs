using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISinWave : MonoBehaviour
{
    // Inspector
    [SerializeField] private float _startOffset = 0;
    [SerializeField] private float _speedScalar = 1;
    [SerializeField] private float _heightScalar = 1;

    // Internal
    private Vector3 _startPos;

    // Collect defaults
    void Start()
    {
        _startPos = this.transform.position;
    }

    // Scaled bobbing
    void Update()
    {
        float yOffset = Mathf.Sin((Time.realtimeSinceStartup + _startOffset) * _speedScalar) * _heightScalar;
        this.transform.position = new Vector3(this.transform.position.x, _startPos.y + yOffset);
    }
}
