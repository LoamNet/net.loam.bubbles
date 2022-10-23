using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayOnToggleOrClick : MonoBehaviour
{
    private void OnMouseEnter()
    {
        Debug.Log("KDSHFJLKJDSf");
    }
    public void AudioOnPlayOnToggleOrClick()
    {
        GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.ButtonOrToggleClick);
    }
}
