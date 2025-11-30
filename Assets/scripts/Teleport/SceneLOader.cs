using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLOader : MonoBehaviour, ISaveable
{
    public Transform playerTrans;

    public UIManager uiManager;

    public GameSceneEventSO firstLoadScene;
    public GameSceneEventSO menuLoadScene;
    public Vector3 firstPosition;
    public Vector3 menuPosition;
    public SceneLoadEventSO loadEventSO;

    private GameSceneEventSO currentLoadScene;
    private GameSceneEventSO locationTOGO;
    public Vector3 positionTOGO;
    private bool fadeScene;
    private bool isLoading;
    public float fadeDuratrion;
    public VoidEventSO afterSceneLoadedEvent;

    public VoidEventSO newGameEvent;

    public FadeEventSO fadeEvent;

    public VoidEventSO BackToMenuEvent;

    //TODO mainmenu
    private void Awake()
    {
    }
    private void Start()
    {
        loadEventSO.RaiseLoadRequestEvent(menuLoadScene, menuPosition, true);
        // Debug.Log("START!");
        // NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.LoadSceneRequestEvent += OnLoadRequestEvent;
        newGameEvent.OnEventRaised += NewGame;
        BackToMenuEvent.OnEventRaised += OnBackMenuEvent;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        loadEventSO.LoadSceneRequestEvent -= OnLoadRequestEvent;
        newGameEvent.OnEventRaised -= NewGame;
        BackToMenuEvent.OnEventRaised -= OnBackMenuEvent;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }

    private void OnBackMenuEvent()
    {
        locationTOGO = menuLoadScene;
        loadEventSO.RaiseLoadRequestEvent(locationTOGO, menuPosition, true);
    }

    public void NewGame()
    {
        // Reset persisted state so new game starts fresh
        if (PersistenceManager.Instance != null)
        {
            PersistenceManager.Instance.ClearAll();
        }

        // 清空内存中的 DataManager 保存数据，避免加载到旧存档
        if (DataManager.instance != null)
        {
            DataManager.instance.ClearSaveData();
        }

        locationTOGO = firstLoadScene;
        // OnLoadRequestEvent(locationTOGO, firstPosition, true);
        loadEventSO.RaiseLoadRequestEvent(locationTOGO, firstPosition, true);
    }


    private void OnLoadRequestEvent(GameSceneEventSO locTOGO, Vector3 posTOGO, bool fade)
    {
        if (isLoading) return;
        isLoading = true;
        locationTOGO = locTOGO;
        positionTOGO = posTOGO;
        fadeScene = fade;
        if (currentLoadScene != null)
        {
            // Debug.Log("1");
            StartCoroutine(UnLoadPreviousScene());
        }
        else
        {
            // Debug.Log("2");
            LoadNewScene();
        }
    }
    private IEnumerator UnLoadPreviousScene()
    {
        // Debug.Log(fadeScene);
        if (fadeScene)
        {
            //TODO fade
            // Debug.Log("?");
            fadeEvent.FadeIn(fadeDuratrion);
            // if (currentLoadScene.sceneTpye == SceneTpye.Location) uiManager.playerStateBar.gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(fadeDuratrion);
        // Debug.Log("fadeComplete!");
        yield return currentLoadScene.sceneRefetence.UnLoadScene();
        playerTrans.gameObject.SetActive(false);
        LoadNewScene();
    }
    private void LoadNewScene()
    {
        var loadingOpt = locationTOGO.sceneRefetence.LoadSceneAsync(LoadSceneMode.Additive, true);
        loadingOpt.Completed += OnLoadComplete;
    }

    private void OnLoadComplete(AsyncOperationHandle<SceneInstance> handle)
    {
        currentLoadScene = locationTOGO;
        playerTrans.position = positionTOGO;
        // Debug.Log(positionTOGO);
        // Debug.Log(playerTrans.position);
        playerTrans.gameObject.SetActive(true);
        if (fadeScene)
        {
            //TODO fade
            fadeEvent.FadeOut(fadeDuratrion);
            if (currentLoadScene.sceneTpye == SceneTpye.Location) uiManager.playerStateBar.gameObject.SetActive(true);
        }
        isLoading = false;
        if (currentLoadScene.sceneTpye == SceneTpye.Location)
            afterSceneLoadedEvent.RaiseEvent();
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        data.SaveGameScene(currentLoadScene);
    }

    public void LoadData(Data data)
    {
        var playerID = playerTrans.GetComponent<DataDefinition>().ID;
        if (data.characterPosDict.ContainsKey(playerID))
        {
            positionTOGO = data.characterPosDict[playerID];
            locationTOGO = data.LoadGameScene();
            // Debug.Log(locationTOGO);
            // Debug.Log(locationTOGO.name);
            OnLoadRequestEvent(locationTOGO, positionTOGO, true);

            // else
            // {
            //     OnLoadRequestEvent(locationTOGO, positionTOGO, false);

            // }
        }
    }
}
