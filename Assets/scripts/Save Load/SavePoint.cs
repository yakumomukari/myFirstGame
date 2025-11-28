using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SavePoint : MonoBehaviour, IInteractable
{
    public VoidEventSO loadGameDataSO;

    private SpriteRenderer spriteRenderer;

    public GameObject lightObj;
    public Sprite closeimage;
    public Sprite openimage;
    public bool isDone;

    [Header("闪光特效设置")]
    public Volume targetVolume;
    private ColorAdjustments colorAdjustments;

    [Tooltip("曝光闪光最大值")]
    public float exposureMax = 2f;

    [Tooltip("闪光总持续时间（秒）")]
    public float flashDuration = 0.5f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 从 Volume 的 profile 中获取 ColorAdjustments
        if (targetVolume != null && targetVolume.profile.TryGet<ColorAdjustments>(out var ca))
        {
            colorAdjustments = ca;
        }
    }

    private void OnEnable()
    {
        spriteRenderer.sprite = isDone ? openimage : closeimage;
        lightObj.SetActive(isDone);
    }

    public void TriggerAction()
    {
        if (!isDone)
        {
            isDone = true;
            spriteRenderer.sprite = openimage;
            this.gameObject.tag = "Untagged";
            lightObj.SetActive(true);

            // 触发闪光特效
            if (colorAdjustments != null)
            {
                StartCoroutine(PlayFlashEffect());
            }

            //TODO save
            loadGameDataSO.RaiseEvent();
        }
    }

    /// <summary>
    /// 播放闪光特效：曝光度从 0 升到最大，再降回 0
    /// </summary>
    IEnumerator PlayFlashEffect()
    {
        float halfDuration = flashDuration * 0.5f;
        float elapsedTime = 0f;
        float initialExposure = colorAdjustments.postExposure.value;

        // 升阶段：从初始曝光升到最大
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / halfDuration;
            colorAdjustments.postExposure.value = Mathf.Lerp(initialExposure, exposureMax, progress);
            yield return null;
        }

        // 确保达到最大值
        colorAdjustments.postExposure.value = exposureMax;

        elapsedTime = 0f;

        // 降阶段：从最大曝光降回初始值
        while (elapsedTime < halfDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / halfDuration;
            colorAdjustments.postExposure.value = Mathf.Lerp(exposureMax, initialExposure, progress);
            yield return null;
        }

        // 确保降回初始值
        colorAdjustments.postExposure.value = initialExposure;
    }
}
