using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerStateBar playerStateBar;
    [Header("Listen")]
    public CharacterEventSO healthEvent;
    public CharacterEventSO powerEvent;
    public SceneLoadEventSO loadEventSO;
    public VoidEventSO loadDataEvent;
    public VoidEventSO gameOverEvent;
    public VoidEventSO backToMenuEvent;
    public FloatEventSO inPausePanelEvent;
    public FloatEventSO syncVolumeEvent;
    public VoidEventSO pauseGameEvent;


    public GameObject gameOverPanel;
    public GameObject restartBTN;
    public GameObject mobileTouch;

    public Button settingButton;
    public GameObject pausePanel;
    public Slider volumeSlider;
    private void Awake()
    {
#if UNITY_STANDALONE
        mobileTouch.SetActive(false);
#endif
        settingButton.onClick.AddListener(TogglePausePanel);
    }

    private void OnEnable()
    {
        healthEvent.OnEventRaised += OnHealthEvent;
        powerEvent.OnEventRaised += OnPowerEvent;
        loadEventSO.LoadSceneRequestEvent += OnLoadEvent;
        loadDataEvent.OnEventRaised += OnLoadDataEvent;
        gameOverEvent.OnEventRaised += OnGameOverEvent;
        backToMenuEvent.OnEventRaised += OnBackToMenuEvent;
        pauseGameEvent.OnEventRaised += TogglePausePanel;
        syncVolumeEvent.OnEventRaised += OnSyncVolumeEvent;
    }

    private void OnDisable()
    {
        healthEvent.OnEventRaised -= OnHealthEvent;
        loadEventSO.LoadSceneRequestEvent -= OnLoadEvent;
        powerEvent.OnEventRaised -= OnPowerEvent;
        loadDataEvent.OnEventRaised -= OnLoadDataEvent;
        gameOverEvent.OnEventRaised -= OnGameOverEvent;
        backToMenuEvent.OnEventRaised -= OnBackToMenuEvent;
        pauseGameEvent.OnEventRaised -= TogglePausePanel;
        syncVolumeEvent.OnEventRaised -= OnSyncVolumeEvent;
    }

    private void OnSyncVolumeEvent(float v)
    {
        volumeSlider.value = (v + 80) / 100;
    }

    private void OnBackToMenuEvent()
    {
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        Time.timeScale = 1;
    }

    private void TogglePausePanel()
    {
        if (pausePanel.activeInHierarchy)
        {
            pausePanel.SetActive(false);
            inPausePanelEvent.RaiseEvent(1f);
            Time.timeScale = 1;
        }
        else
        {
            pausePanel.SetActive(true);
            inPausePanelEvent.RaiseEvent(0f);
            Time.timeScale = 0;
        }
    }

    private void OnLoadDataEvent()
    {
        gameOverPanel.SetActive(false);
    }

    private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartBTN);
    }

    private void OnLoadEvent(GameSceneEventSO sceneTOGO, Vector3 arg1, bool arg2)
    {
        if (sceneTOGO.sceneTpye == SceneTpye.Menu)
        {
            playerStateBar.gameObject.SetActive(false);
        }
        // if (sceneTOGO.sceneTpye == SceneTpye.Location)
        // {
        //     playerStateBar.gameObject.SetActive(true);
        // }
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
