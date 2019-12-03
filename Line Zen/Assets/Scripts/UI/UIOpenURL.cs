using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIOpenURL : MonoBehaviour
{
    public string targetURL;
    private Button button;
    void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(() => { Application.OpenURL(targetURL); });
    }
}
