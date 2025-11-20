using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public PlayerInputControl inputControl;
	private Rigidbody2D rb;
	public Vector2 inputDirection;
	private PhysicsCheck physicsCheck;
	private PlayerAnimations playerAnimations;
	private Character character;
	private Collider2D coll;
	private ImageShake imageShake;
	[Header("基本参数")]
	public float speed;
	public float jumpForce;
	public float hurtForce;
	public float slideForce;
	public float powerConsume;
	public float wallSlideSpeed;
	public float wallJumpForce;
	public float wallJumpTime;
	public float wallJumpTimeCounter;

	[Header("物理材质")]
	public PhysicsMaterial2D normal;
	public PhysicsMaterial2D slide;
	[Header("状态")]
	public bool isHurt;
	public bool isDead;
	public bool isAttack;
	public bool isSlide;
	public bool isDamageInvulnerable;
	public bool isPushWall;
	public bool isWallSlide;
	public bool isWallJump;
	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		physicsCheck = GetComponent<PhysicsCheck>();
		playerAnimations = GetComponent<PlayerAnimations>();
		inputControl = new PlayerInputControl();
		coll = GetComponent<Collider2D>();
		character = GetComponent<Character>();
		imageShake = GetComponent<ImageShake>();

		// jump
		inputControl.Gameplay.Jump.started += Jump;

		// attack
		inputControl.Gameplay.Attack.started += PlayerAttack;
		// slide
		inputControl.Gameplay.Slide.started += PlayerSlide;

		wallJumpTimeCounter = wallJumpTime;
	}


	private void OnEnable()
	{
		inputControl.Enable();
	}
	private void OnDisable()
	{
		inputControl.Disable();
	}
	private void Update()
	{
		inputDirection = inputControl.Gameplay.Move.ReadValue<Vector2>();
		// Debug.Log(inputDirection);
		CheckState();
	}
	private void FixedUpdate()
	{
		if (!isHurt && !isAttack && !isSlide && !isWallJump)
			Move();
		SlideWall();
		WallJumpCount();
	}

	public void SlideWall()
	{
		if (!physicsCheck.isGround && physicsCheck.isWall && inputDirection.x != 0)
			isPushWall = true;
		else isPushWall = false;
		if (isPushWall && rb.velocity.y < 0) isWallSlide = true;
		else isWallSlide = false;
		if (isWallSlide)
		{
			rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
		}
	}

	// test
	// private void OnTriggerStay2D(Collider2D enemy)
	// {
	// 	// Debug.Log(enemy.name);
	// }
	public void Move()
	{
		// 行走
		rb.velocity = new Vector2(inputDirection.x * speed * Time.deltaTime, rb.velocity.y);
		// rb.AddForce(inputDirection * speed * Time.deltaTime, ForceMode2D.Force);
		// 翻转
		int faceDir = (int)transform.localScale.x;
		if (inputDirection.x > 0) faceDir = 1;
		if (inputDirection.x < 0) faceDir = -1;
		transform.localScale = new Vector3(faceDir, 1, 1);
	}
	private void PlayerSlide(InputAction.CallbackContext obj)
	{
		if (character.currentPower >= powerConsume)
		{
			if (physicsCheck.isGround && !isAttack && !isSlide)
			{
				playerAnimations.PlaySlide();
				character.currentPower -= powerConsume;
				isSlide = true;
				Silde();
				character.OnPowerChange?.Invoke(character);
			}
		}
		else
		{
			//TODO:冲刺条抖动
			// imageShake.StartShake();
		}
	}
	private void PlayerAttack(InputAction.CallbackContext obj)
	{
		if (!isSlide && !isWallSlide)
		{
			playerAnimations.PlayAttack();
			isAttack = true;
		}
	}
	private void Jump(InputAction.CallbackContext obj)
	{
		// Debug.Log(transform.position);
		// Debug.Log(this.gameObject.transform.position);
		if (physicsCheck.isGround)
		{
			// Vector2 dir = new Vector2(transform.localScale.x, 0).normalized;
			// rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
			rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
			GetComponent<AudioDefinition>()?.PlayAudioClip();
		}
		if (isWallSlide)
		{
			int faceDir = (int)transform.localScale.x;
			if (inputDirection.x > 0) faceDir = 1;
			if (inputDirection.x < 0) faceDir = -1;
			transform.localScale = new Vector3(-faceDir, 1, 1);
			wallJumpTimeCounter = wallJumpTime;
			isWallJump = true;
			rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
			// Debug.Log(transform.up);
			rb.AddForce(new Vector2(transform.localScale.x, 0) * wallJumpForce, ForceMode2D.Impulse);
			GetComponent<AudioDefinition>()?.PlayAudioClip();
		}
	}
	private void WallJumpCount()
	{
		if (wallJumpTimeCounter > 0)
		{
			wallJumpTimeCounter -= Time.deltaTime;
		}
		else
		{
			isWallJump = false;
		}
	}
	public void GetHurt(Transform attakcer)
	{
		isHurt = true;
		isDamageInvulnerable = true;
		rb.velocity = Vector2.zero;
		Vector2 dir = new Vector2(transform.position.x - attakcer.transform.position.x, 0).normalized;
		rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
	}
	public void PlayerDead()
	{
		isDead = true;
		inputControl.Gameplay.Disable();
	}
	private void CheckState()
	{
		if (physicsCheck.isGround) coll.sharedMaterial = normal;
		if (physicsCheck.isWall || !physicsCheck.isGround) coll.sharedMaterial = slide;
	}
	private void Silde()
	{
		rb.velocity = new Vector2(0, rb.velocity.y);
		if (!character.invulnerable)
			character.invulnerable = true;
		Vector2 dir = new Vector2(transform.localScale.x, 0);
		rb.AddForce(dir * slideForce, ForceMode2D.Impulse);
	}

}
