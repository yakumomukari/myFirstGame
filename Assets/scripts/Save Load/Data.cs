using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    public string sceneToSave;
    public string sceneName;
    public Dictionary<string, Vector3> characterPosDict = new Dictionary<string, Vector3>();
    public Dictionary<string, float> floatDict = new Dictionary<string, float>();

    public void SaveGameScene(GameSceneEventSO savedScene)
    {
        sceneToSave = JsonUtility.ToJson(savedScene);
        sceneName = savedScene.name;
    }
    public GameSceneEventSO LoadGameScene()
    {
        var newScene = ScriptableObject.CreateInstance<GameSceneEventSO>();
        JsonUtility.FromJsonOverwrite(sceneToSave, newScene);
        return newScene;
    }
}
