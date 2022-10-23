using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdateVolume : MonoBehaviour
{
    [SerializeField] public Events _events;
    [SerializeField] public GameAudio.SoundCategory _category;
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private TMPro.TextMeshProUGUI _text;

    // Start is called before the first frame update
    void Start()
    {
        _slider.onValueChanged.AddListener((newVal) => {
            float normalized = newVal / (float)_slider.maxValue;
            if (_category == GameAudio.SoundCategory.MUSIC)
            {
                _events.OnUpdateMusicVolume?.Invoke(normalized);
            }
            else if (_category == GameAudio.SoundCategory.SFX)
            {
                _events.OnUpdateSFXVolume?.Invoke(normalized);
            }

            _text.text = (newVal * _slider.maxValue).ToString();
        });

        _events.OnDataChanged += (data) => {
            if (_category == GameAudio.SoundCategory.MUSIC)
            {
                _slider.value = data.musicVolume * _slider.maxValue;
            }
            else
            {
                _slider.value = data.sfxVolume * _slider.maxValue;
            }

            _text.text = (_slider.value * _slider.maxValue).ToString();
        };
    }
}

