using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIConstructTextAsset : MonoBehaviour
{
    public GameCore core;
    [Space]
    public Events events;
    public TMPro.TMP_InputField input;
    public Button button;

    // Start is called before the first frame update
    void OnEnable()
    {
        button.onClick.AddListener(OnLoadRequest);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(OnLoadRequest);
    }

    void OnLoadRequest()
    {
        TextAsset loaded = new TextAsset(input.text);
        core.internalLevel = loaded;
    }

    private void Update()
    {
        if (core.NumberOfBubbles > 0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
