using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class ImageShake : MonoBehaviour
{
    [Header("抖动参数")]
    public float shakeDuration;      // 抖动持续时间
    public float shakeIntensity;      // 抖动强度
    public float shakeFrequency;      // 抖动频率（每秒抖动次数）

    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private bool isShaking = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        originalPosition = rectTransform.anchoredPosition;
    }

    // 开始抖动
    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }

    // 带参数的开始抖动
    public void StartShake(float duration, float intensity)
    {
        if (!isShaking)
        {
            StartCoroutine(ShakeCoroutine(duration, intensity));
        }
    }

    private IEnumerator ShakeCoroutine()
    {
        return ShakeCoroutine(shakeDuration, shakeIntensity);
    }

    private IEnumerator ShakeCoroutine(float duration, float intensity)
    {
        isShaking = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            // 计算衰减系数（随时间减弱）
            float decay = 1f - (elapsed / duration);

            // 生成随机偏移
            float offsetX = Random.Range(-1f, 1f) * intensity * decay;
            float offsetY = Random.Range(-1f, 1f) * intensity * decay;

            // 应用偏移
            rectTransform.anchoredPosition = originalPosition + new Vector2(offsetX, offsetY);

            // 控制频率
            yield return new WaitForSeconds(1f / shakeFrequency);
        }

        // 恢复原始位置
        rectTransform.anchoredPosition = originalPosition;
        isShaking = false;
    }
}