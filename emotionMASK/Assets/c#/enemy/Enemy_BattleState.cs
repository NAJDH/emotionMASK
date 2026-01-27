using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_BattleState : EnemyState
{
    public Enemy_BattleState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    
}
