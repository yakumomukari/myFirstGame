using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
	private PlayerController playerController;
	[Header("基本属性")]
	public float maxHealth;
	public float currentHealth;
	public float maxPower;
	public float currentPower;
	[Header("受伤无敌")]
	public float invulnerableDuration;
	private float invulnerableCounter;
	public bool invulnerable;
	public UnityEvent<Transform> OnTakeDamage;
	public UnityEvent<Character> OnHealthChange;
	public UnityEvent<Character> OnPowerChange;
	public UnityEvent OnDie;
	private void Awake()
	{
		playerController = GetComponent<PlayerController>();
	}
	private void Start()
	{
		currentHealth = maxHealth;
		OnHealthChange?.Invoke(this);
		currentPower = maxPower;
		OnPowerChange?.Invoke(this);
	}
	private void Update()
	{
		if (currentPower < maxPower)
		{
			currentPower += Time.deltaTime * 10;
			OnPowerChange?.Invoke(this);
		}
		if (invulnerable && !playerController.isSlide)
		{
			invulnerableCounter -= Time.deltaTime;
			if (invulnerableCounter <= 0)
			{
				invulnerable = false;
				playerController.isDamageInvulnerable = false;
			}
		}
	}
	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.CompareTag("Water"))
		{
			currentHealth = 0;
			OnHealthChange?.Invoke(this);
			OnDie?.Invoke();
		}
		//TODO spike
		// if (other.CompareTag("Spike"))
		// {

		// }
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
		OnHealthChange?.Invoke(this);
	}
	public void TriggerInvulnerable()
	{
		if (!invulnerable)
		{
			invulnerable = true;
			invulnerableCounter = invulnerableDuration;
		}
	}
}
