using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarWalkState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        if ((!currentEnemy.physicsCheck.isGround) || currentEnemy.physicsCheck.leftWall)
        {
            currentEnemy.wait = true;
            currentEnemy.rb.velocity = new Vector2(0, currentEnemy.rb.velocity.y);
            // transform.localScale = new Vector3(faceDir.x, 1, 1);
            currentEnemy.anim.SetBool("walk", false);
        }
        else
        {
            currentEnemy.anim.SetBool("walk", true);

        }
    }



    public override void PhysicsUpdate()
    {
    }
    public override void OnExit()
    {
        // currentEnemy.lostTimeCounter = currentEnemy.lostTime;
        currentEnemy.anim.SetBool("walk", false);
    }
}
