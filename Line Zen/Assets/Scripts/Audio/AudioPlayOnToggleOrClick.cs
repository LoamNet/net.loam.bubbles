using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayOnToggleOrClick : MonoBehaviour
{
    public void AudioOnPlayOnToggleOrClick()
    {
        GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.ButtonOrToggleClick);
    }
}
