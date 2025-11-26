using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/FadeEventSO")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<Color, float, bool> OnEventRaised;
    public void FadeIn(float duration)
    {
        RaiseEvent(Color.black, duration, true);
        // Debug.Log("in");
    }
    public void FadeOut(float duration)
    {
        RaiseEvent(Color.clear, duration, false);
        // Debug.Log("out");
    }
    public void RaiseEvent(Color target, float duration, bool isIn)
    {
        OnEventRaised?.Invoke(target, duration, isIn);
    }
}
