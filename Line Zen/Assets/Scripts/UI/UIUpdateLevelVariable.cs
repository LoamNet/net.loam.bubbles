using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdateLevelVariable : MonoBehaviour
{
    public Events events;

    // Start is called before the first frame update
    void Start()
    {
        events.OnSerializedDataChange += (data) => {
            GetComponent<TMPro.TextMeshProUGUI>().text = "" + data.level;
        };
    }
}
