using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;
    public FadeEventSO fadeEvent;
    private void Awake()
    {
        fadeImage.gameObject.SetActive(true);
    }
    private void OnEnable()
    {
        fadeEvent.OnEventRaised += OnFadeEvent;
    }
    private void OnDisable()
    {
        fadeEvent.OnEventRaised -= OnFadeEvent;
    }
    public void OnFadeEvent(Color targetcol, float duration, bool isIn)
    {
        fadeImage.DOBlendableColor(targetcol, duration);
        // Debug.Log("FADE");
    }
}
