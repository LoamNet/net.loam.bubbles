using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdateVolume : MonoBehaviour
{
    [SerializeField] public Events _events;
    [SerializeField] public GameAudio.SoundCategory _category;
    [SerializeField] private UnityEngine.UI.Slider _slider;
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    [SerializeField] private GameCore core;

    // Start is called before the first frame update
    private void Awake()
    {
        _events.OnDataChanged += OnDataChange;
        _slider.onValueChanged.AddListener(OnSliderChange);
    }

    private void OnSliderChange(float newVal)
    {
        float normalized = newVal / (float)_slider.maxValue;
        if (_category == GameAudio.SoundCategory.MUSIC)
        {
            _events.OnUpdateMusicVolume?.Invoke(normalized);
        }
        else if (_category == GameAudio.SoundCategory.SFX)
        {
            _events.OnUpdateSFXVolume?.Invoke(normalized);

            if (core != null && core.State == GameState.Options)
            {
                GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.SliderTick);
            }
        }

        _text.text = (newVal * _slider.maxValue).ToString();
    }

    private void OnDataChange(DataGeneral data)
    {
        if (_category == GameAudio.SoundCategory.MUSIC)
        {
            _slider.value = data.musicVolume * _slider.maxValue;
        }
        else
        {
            _slider.value = data.sfxVolume * _slider.maxValue;
        }

        _text.text = (_slider.value * _slider.maxValue).ToString();
    }
}

