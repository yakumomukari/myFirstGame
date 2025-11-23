using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour, IInteractable
{
    public GameSceneEventSO gameSceneTOGO;
    public SceneLoadEventSO LoadEventSO;
    public Vector3 teleportTOGO;
    public void TriggerAction()
    {

        LoadEventSO.RaiseLoadRequestEvent(gameSceneTOGO, teleportTOGO, true);
        // Debug.Log("1");
    }
}
