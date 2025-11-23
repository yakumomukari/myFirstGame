using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class FadeCanvas : MonoBehaviour
{
    public Image fadeImage;

    public void OnFadeEvent(Color targetcol, float duration)
    {
        fadeImage.DOBlendableColor(targetcol, duration);
    }
}
