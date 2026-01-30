using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        enemybase.SetZeroVelocity();
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        if (movementOver)
            stateMachine.ChangeState(enemybase.battleState);
    }
}
