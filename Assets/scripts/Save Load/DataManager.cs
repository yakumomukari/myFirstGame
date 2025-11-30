using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(order: -100)]
public class DataManager : MonoBehaviour
{
    public PlayerController playerController;
    public static DataManager instance;
    public VoidEventSO saveDataEvent;
    public VoidEventSO loadDataEvent;
    public VoidEventSO playLoadButtonEvent;
    private List<ISaveable> saveList = new List<ISaveable>();
    private Data saveData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        saveData = new Data();
    }

    /// <summary>
    /// 清空当前内存中的保存数据（用于新游戏时重置）
    /// </summary>
    public void ClearSaveData()
    {
        saveData = new Data();
    }

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
        loadDataEvent.OnEventRaised += Load;
        playLoadButtonEvent.OnEventRaised += Load;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
        loadDataEvent.OnEventRaised -= Load;
        playLoadButtonEvent.OnEventRaised -= Load;
    }
    public void RegisterSaveData(ISaveable saveable)
    {
        if (!saveList.Contains(saveable))
        {
            saveList.Add(saveable);
        }
    }
    public void UnRegisterSavaData(ISaveable saveable)
    {
        if (saveList.Contains(saveable))
        {
            saveList.Remove(saveable);
        }
    }
    public void Save()
    {
        foreach (var saveable in saveList)
        {
            saveable.GetSaveData(saveData);
        }
        foreach (var item in saveData.characterPosDict)
        {
            // Debug.Log(item.Key + "     " + item.Value);
        }
    }
    public void Load()
    {
        foreach (var saveable in saveList)
        {
            saveable.LoadData(saveData);
        }
    }
}
