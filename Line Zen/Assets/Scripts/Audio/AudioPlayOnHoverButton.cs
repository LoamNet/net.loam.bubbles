using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayOnHoverButton : MonoBehaviour
{
    public void AudioOnPlayEffectButton()
    {
        GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.Button);
    }
}
