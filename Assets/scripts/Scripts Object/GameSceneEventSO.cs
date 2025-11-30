using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "Game Scene/GameSceneEventSO")]
public class GameSceneEventSO : ScriptableObject
{
    public SceneTpye sceneTpye;
    public AssetReference sceneRefetence;
    // public string sceneName;
}