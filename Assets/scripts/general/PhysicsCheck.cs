using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
	// public Vector2 wallOffset1, wallOffset2, wallOffset3, wallOffset4;
	public Vector2 bottomOffset;
	public Vector2 leftOffset, rightOffset, wallOffset;
	public LayerMask groundLayer;
	public bool isGround;
	public bool isWall;
	public bool leftWall;
	public bool rightWall;
	public float checkRaduis;
	private void Update()
	{
		Check();
	}
	public void Check()
	{
		// check ground
		isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
		// check wall
		isWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(wallOffset.x * transform.localScale.x, wallOffset.y), checkRaduis, groundLayer);
		// isWall = Physics2D.OverlapArea((Vector2)transform.position + wallOffset1, (Vector2)transform.position + wallOffset2, groundLayer) || Physics2D.OverlapArea((Vector2)transform.position + wallOffset3, (Vector2)transform.position + wallOffset4, groundLayer);
		leftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x * transform.localScale.x, leftOffset.y), checkRaduis, groundLayer);
		rightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x * transform.localScale.x, rightOffset.y), checkRaduis, groundLayer);
	}
	private void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
		// Gizmos.DrawLine((Vector2)transform.position + wallOffset1, (Vector2)transform.position + wallOffset2);
		Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(wallOffset.x * transform.localScale.x, wallOffset.y), checkRaduis);
		// Gizmos.DrawLine((Vector2)transform.position + wallOffset3, (Vector2)transform.position + wallOffset4);
		Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x * transform.localScale.x, leftOffset.y), checkRaduis);
		Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x * transform.localScale.x, rightOffset.y), checkRaduis);
	}
}
