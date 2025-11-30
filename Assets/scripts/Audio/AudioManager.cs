using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public PlayAudioEventSO FXEvent;
    public PlayAudioEventSO BGMEvent;
    public FloatEventSO volumeChangeEvent;
    public FloatEventSO inPausePanelEvent;
    public FloatEventSO syncVolumeEvent;
    public AudioSource BGMSource;
    public AudioSource FXSource;

    public AudioMixer mixer;
    private void OnEnable()
    {
        FXEvent.OnEventRaised += OnFXEvent;
        BGMEvent.OnEventRaised += OnBGMEvent;
        volumeChangeEvent.OnEventRaised += OnVolumeChangeEvent;
        inPausePanelEvent.OnEventRaised += OnPauseEvent;
    }

    private void OnDisable()
    {
        FXEvent.OnEventRaised -= OnFXEvent;
        BGMEvent.OnEventRaised -= OnBGMEvent;
        volumeChangeEvent.OnEventRaised -= OnVolumeChangeEvent;
        inPausePanelEvent.OnEventRaised -= OnPauseEvent;
    }

    private void OnPauseEvent(float arg0)
    {
        float v;
        mixer.GetFloat("MasterVolume", out v);
        syncVolumeEvent.RaiseEvent(v);
    }

    private void OnVolumeChangeEvent(float v)
    {
        mixer.SetFloat("MasterVolume", v * 100 - 80);
    }

    private void OnBGMEvent(AudioClip clip)
    {
        BGMSource.clip = clip;
        BGMSource.Play();
    }

    private void OnFXEvent(AudioClip clip)
    {
        FXSource.PlayOneShot(clip);
    }
}
