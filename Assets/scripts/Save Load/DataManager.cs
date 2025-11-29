using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public VoidEventSO saveDataEvent;
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

    private void OnEnable()
    {
        saveDataEvent.OnEventRaised += Save;
    }
    private void OnDisable()
    {
        saveDataEvent.OnEventRaised -= Save;
    }
    private void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            Load();
            // Debug.Log("fuck");
        }
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
