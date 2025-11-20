using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[CreateAssetMenu(menuName = "Event/SceneLoadEventSO")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<GameSceneEventSO, Vector3, bool> LoadSceneRequestEvent;
    public void RaiseLoadRequestEvent(GameSceneEventSO locationgTOGO, Vector3 positionTOGO, bool fadeScreen)
    {
        // Debug.Log("2");
        LoadSceneRequestEvent?.Invoke(locationgTOGO, positionTOGO, fadeScreen);
    }
}
