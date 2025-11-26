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
    public SceneLoadEventSO loadEventSO;
    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
        loadEventSO.LoadSceneRequestEvent += OnLoadEvent;
    }


    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEventSO.LoadSceneRequestEvent -= OnLoadEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
    }
    private void OnLoadEvent(GameSceneEventSO sceneTOGO, Vector3 arg1, bool arg2)
    {
        if (sceneTOGO.sceneTpye == SceneTpye.Menu)
        {
            playerStateBar.gameObject.SetActive(false);
        }
        if (sceneTOGO.sceneTpye == SceneTpye.Location)
        {
            playerStateBar.gameObject.SetActive(true);
        }
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
