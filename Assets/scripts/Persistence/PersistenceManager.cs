using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 跨场景保存简单状态（使用 PlayerPrefs 持久化），并在场景间保持单例。
/// 用法：PersistenceManager.Instance.SetBool(id, true/false);
///       bool v = PersistenceManager.Instance.GetBool(id, false);
/// </summary>
public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }

    private const string KeyPrefix = "PM:";

    private Dictionary<string, bool> cache = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureExists()
    {
        if (Instance == null)
        {
            var go = new GameObject("_PersistenceManager");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<PersistenceManager>();
        }
    }

    public bool GetBool(string id, bool defaultValue = false)
    {
        if (string.IsNullOrEmpty(id)) return defaultValue;

        if (cache.TryGetValue(id, out var v)) return v;

        string key = KeyPrefix + id;
        if (PlayerPrefs.HasKey(key))
        {
            int iv = PlayerPrefs.GetInt(key, defaultValue ? 1 : 0);
            v = iv != 0;
        }
        else
        {
            v = defaultValue;
        }

        cache[id] = v;
        return v;
    }

    public void SetBool(string id, bool value)
    {
        if (string.IsNullOrEmpty(id)) return;
        cache[id] = value;
        string key = KeyPrefix + id;
        PlayerPrefs.SetInt(key, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ClearAll()
    {
        cache.Clear();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}
