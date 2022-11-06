using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioPlayOnToggleOrClick : MonoBehaviour, IPointerClickHandler
{
    // Set to true to use unity monobehavior built in events instead of using event triggers, etc
    [SerializeField] private bool useInternalEvents = false;


    public void AudioOnPlayOnToggleOrClick()
    {
        if (!useInternalEvents)
        {
            GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.ButtonOrToggleClick);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (useInternalEvents)
        {
            GameAudio.Instance.PlaySoundEffect(GameAudio.Instance.ButtonOrToggleClick);
        }
    }
}
