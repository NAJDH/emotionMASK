using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : EnemyState
{
    public Enemy_IdleState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemybase.idleTime;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemybase.moveState);
    }
}
