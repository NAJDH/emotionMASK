using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }


    public override void Update()
    {
        base.Update();
        
        if (enemybase.PlayerDetected())
            stateMachine.ChangeState(enemybase.battleState);
    }
}
