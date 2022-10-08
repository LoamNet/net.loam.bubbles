using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPushToTestLevel : MonoBehaviour
{
    [SerializeField] private string _target = "LevelRunner";
    [SerializeField] private GameEditorUtility _editor;
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
        string saved = _editor.Serialize();
        GameEditorRepository.Instance.LevelDataFromEditor = saved;
        SceneManager.LoadScene(_target);
    }
}
