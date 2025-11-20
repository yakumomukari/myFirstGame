using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDefinition : MonoBehaviour
{
    public PlayAudioEventSO playAudioEvent;
    public AudioClip audioClip;
    public bool playOnEnable;
    private void OnEnable()
    {
        if (playOnEnable)
        {
            PlayAudioClip();
        }
    }
    public void PlayAudioClip()
    {
        playAudioEvent.RaiseEvent(audioClip);
    }
}
