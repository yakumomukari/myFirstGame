using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    public Image healthImage;
    public Image healthDelayImage;
    public Image powerImage;
    private void Update()
    {
        if (healthDelayImage.fillAmount > healthImage.fillAmount)
        {
            healthDelayImage.fillAmount -= Time.deltaTime * 0.1f;
        }
        else
        {
            healthDelayImage.fillAmount = healthImage.fillAmount;
        }
        // if (powerImage.fillAmount != 1)
        // {
        //     powerImage.fillAmount += Time.deltaTime;
        // }

    }
    public void OnHealthChange(float persentage)
    {
        healthImage.fillAmount = persentage;
    }
    public void OnPowerChange(float persentage)
    {
        powerImage.fillAmount = persentage;
    }
}
