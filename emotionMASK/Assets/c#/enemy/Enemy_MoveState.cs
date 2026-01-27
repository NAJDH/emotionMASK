using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!enemybase.isGrounded || enemybase.isTouchingTheWall)
            enemybase.Flip();
    }

    public override void Update()
    {
        base.Update();

        enemybase.SetVelocity(enemybase.moveSpeed * enemybase.EntityDirection, enemybase.rb.velocity.y);

        if (!enemybase.isGrounded || enemybase.isTouchingTheWall)
            stateMachine.ChangeState(enemybase.idleState);
    }
}
