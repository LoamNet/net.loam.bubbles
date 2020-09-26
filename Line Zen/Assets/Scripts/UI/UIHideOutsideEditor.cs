using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHideOutsideEditor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
#if !UNITY_EDITOR
        Destroy(this.gameObject);
#endif
    }
}
