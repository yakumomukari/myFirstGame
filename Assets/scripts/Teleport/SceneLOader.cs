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
    public Transform playerTrans
    ;
    public GameSceneEventSO firstLoadScene;
    public SceneLoadEventSO loadEventSO;

    private GameSceneEventSO currentLoadScene;
    private GameSceneEventSO locationTOGO;
    public Vector3 positionTOGO;
    private bool fadeScene;
    private bool isLoading;
    public float fadeDuratrion;
    private void Awake()
    {
        // Addressables.LoadSceneAsync(firstLoadScene.sceneRefetence, LoadSceneMode.Additive);
        currentLoadScene = firstLoadScene;
        currentLoadScene.sceneRefetence.LoadSceneAsync(LoadSceneMode.Additive);
    }
    private void OnEnable()
    {
        loadEventSO.LoadSceneRequestEvent += OnLoadRequestEvent;
    }
    private void OnDisable()
    {
        loadEventSO.LoadSceneRequestEvent -= OnLoadRequestEvent;

    }

    private void OnLoadRequestEvent(GameSceneEventSO locTOGO, Vector3 posTOGO, bool fade)
    {
        locationTOGO = locTOGO;
        positionTOGO = posTOGO;
        fadeScene = fade;

        // Debug.Log(locationTOGO.sceneRefetence.SubObjectName);
        // Debug.Log("?");
        if (currentLoadScene != null)
            StartCoroutine(UnLoadPreviousScene());
    }
    private IEnumerator UnLoadPreviousScene()
    {
        if (fadeScene)
        {
            //TODO fade
        }
        yield return new WaitForSeconds(fadeDuratrion);
        yield return currentLoadScene.sceneRefetence.UnLoadScene();
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
        if (fadeScene)
        {
            //TODO fade
        }
    }
}
