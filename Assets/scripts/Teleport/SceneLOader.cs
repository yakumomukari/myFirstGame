using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public class SceneLOader : MonoBehaviour
{
    public Transform playerTrans;
    public GameSceneEventSO firstLoadScene;
    public Vector3 firstPosition;
    public SceneLoadEventSO loadEventSO;

    private GameSceneEventSO currentLoadScene;
    private GameSceneEventSO locationTOGO;
    public Vector3 positionTOGO;
    private bool fadeScene;
    private bool isLoading;
    public float fadeDuratrion;
    public VoidEventSO afterSceneLoadedEvent;

    public FadeEventSO fadeEvent;
    //TODO mainmenu
    private void Start()
    {
        // Debug.Log("START!");
        NewGame();
    }
    private void OnEnable()
    {
        loadEventSO.LoadSceneRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        loadEventSO.LoadSceneRequestEvent -= OnLoadRequestEvent;

    }

    public void NewGame()
    {
        locationTOGO = firstLoadScene;
        OnLoadRequestEvent(locationTOGO, firstPosition, true);
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
        Debug.Log(fadeScene);
        if (fadeScene)
        {
            //TODO fade
            Debug.Log("?");
            fadeEvent.FadeIn(fadeDuratrion);
        }
        yield return new WaitForSeconds(fadeDuratrion);
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
        }
        isLoading = false;
        afterSceneLoadedEvent.RaiseEvent();
    }
}
