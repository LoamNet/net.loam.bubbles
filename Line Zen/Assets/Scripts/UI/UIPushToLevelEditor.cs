using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPushToLevelEditor : MonoBehaviour
{
    [SerializeField] private string _target = "LevelEditor";
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    void OnEnable()
    {
        _button.onClick.AddListener(Export);
    }

    void OnDisable()
    {
        _button.onClick.RemoveListener(Export);
    }

    private void Export()
    {
        // All data configured by the time we get to this point (should be)
        SceneManager.LoadScene(_target);
    }
}

