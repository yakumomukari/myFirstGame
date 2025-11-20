using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("Listen")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO powerEvent;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
    }


    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
    }
    private void OnPowerEvent(Character obj)
    {
        var persentage = obj.currentPower / obj.maxPower;
        playerStateBar.OnPowerChange(persentage);
    }
    private void OnHealthEvent(Character obj)
    {
        var persentage = obj.currentHealth / obj.maxHealth;
        playerStateBar.OnHealthChange(persentage);
    }
}
