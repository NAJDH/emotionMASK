using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy_MoveState : EnemyState
{
    public Enemy_MoveState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();

        enemybase.SetVelocity(enemybase.moveSpeed * enemybase.EntityDirection, enemybase.rb.velocity.y);

        if (enemybase.isGrounded == false)
        {
            stateMachine.ChangeState(enemybase.idleState);
            enemybase.Flip();
        }
    }
}
