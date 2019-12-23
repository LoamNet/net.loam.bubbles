using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class UIAdvanceToSceneOnClick : MonoBehaviour
{
    public string targetScene; 

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            SceneManager.LoadScene(targetScene);
        });
    }
}
