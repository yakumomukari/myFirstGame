using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCharacter : MonoBehaviour, ISaveable
{
    private PlayerController playerController;
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    public bool invulnerable;
    public UnityEvent<Transform> OnTakeDamage;
    public UnityEvent OnDie;

    public VoidEventSO newGameEvent;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
        ISaveable saveable = this;
        saveable.RegisterSaveData();
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
        ISaveable saveable = this;
        saveable.UnRegisterSaveData();
    }
    public void NewGame()
    {
        currentHealth = maxHealth;
        // Debug.Log("zhu man xie");
        // OnHealthChange?.Invoke(this);
        // currentPower = maxPower;
        // OnPowerChange?.Invoke(this);
    }
    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(Attack attacker)
    {
        // Debug.Log(attacker.damage);
        if (invulnerable) return;
        if (currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //performing damage
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else
        {
            currentHealth = 0;
            // death
            OnDie?.Invoke();
        }
    }
    public void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public DataDefinition GetDataID()
    {
        return GetComponent<DataDefinition>();
    }

    public void GetSaveData(Data data)
    {
        var nowID = GetDataID().ID;
        if (data.characterPosDict.ContainsKey(nowID))
        {
            data.floatDict[nowID + "health"] = this.currentHealth;
            data.characterPosDict[nowID] = transform.position;
        }
        else
        {
            data.floatDict.Add(nowID + "health", this.currentHealth);
            data.characterPosDict.Add(nowID, transform.position);
        }
    }

    public void LoadData(Data data)
    {
        var nowID = GetDataID().ID;
        if (data.characterPosDict.ContainsKey(nowID))
        {
            transform.position = data.characterPosDict[nowID];
            this.currentHealth = data.floatDict[nowID + "health"];
        }
    }
}
