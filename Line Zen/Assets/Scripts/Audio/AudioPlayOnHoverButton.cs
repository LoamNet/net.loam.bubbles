using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioPlayOnHoverButton : MonoBehaviour, IPointerEnterHandler
{
    // Set to true to use unity monobehavior built in events instead of using event triggers, etc
    [SerializeField] private bool useInternalEvents = false;

    public void AudioOnPlayEffectButton()
    {
        if (!useInternalEvents)
        {
            GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.Button);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (useInternalEvents)
        {
            GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.Button);
        }
    }
}
