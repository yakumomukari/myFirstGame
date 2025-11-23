using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
	private Animator anim;
	private Rigidbody2D rb;
	private PhysicsCheck physicsCheck;
	private PlayerController playerController;
	private void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		physicsCheck = GetComponent<PhysicsCheck>();
		playerController = GetComponent<PlayerController>();
	}
	private void Update()
	{
		// Debug.Log(playerController.isDead);
		SetAnimation();
	}
	public void SetAnimation()
	{
		anim.SetFloat("velocityX", MathF.Abs(rb.velocity.x));
		anim.SetFloat("velocityY", rb.velocity.y);
		anim.SetBool("isGround", physicsCheck.isGround);
		anim.SetBool("isDead", playerController.isDead);
		anim.SetBool("isAttack", playerController.isAttack);
		anim.SetBool("isWall", playerController.isWallSlide);
	}
	public void PlayHurt()
	{
		anim.SetTrigger("takeDamage");
	}
	public void PlayAttack()
	{
		anim.SetTrigger("attack");
	}
	public void PlaySlide()
	{
		anim.SetTrigger("slide");
	}
}
