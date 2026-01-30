using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_BattleState : EnemyState
{
    public Transform player;
    private float lastTimeInBattle;

    public Enemy_BattleState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (player != null)
        {
            player = enemybase.PlayerDetected().transform;
        }

        if(ShouldRetreat())
        {
            enemybase.rb.velocity = new Vector2(enemybase.retreatVelocity.x * FacingDirectionToPlayer(), enemybase.retreatVelocity.y);
            enemybase.FilpController(FacingDirectionToPlayer());
        }

    }

    public override void Update()
    {
        base.Update();

        if(enemybase.PlayerDetected())
            UpdateLastBattleTime();

        if (BattleTimeOut())
            stateMachine.ChangeState(enemybase.idleState);


        if (WithinTheAttackDistance() && enemybase.PlayerDetected())
            stateMachine.ChangeState(enemybase.attackState);
        else
            enemybase.SetVelocity(enemybase.battleMoveSpeed * FacingDirectionToPlayer(), enemybase.rb.velocity.y);
    }


    
    private bool WithinTheAttackDistance()
    {
        return DistanceToPlayer() < enemybase.attackDistance;
    }

    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemybase.transform.position.x);
    }

    private int FacingDirectionToPlayer()
    {
        if (player == null)
            return 0;

        return player.position.x > enemybase.transform.position.x ? 1 : -1;
    }

    private bool ShouldRetreat()
    {
        return DistanceToPlayer() < enemybase.minRetreatDistance;
    }

    private void UpdateLastBattleTime()
    {
        lastTimeInBattle = Time.time;
    }

    private bool BattleTimeOut()
    {
        return Time.time > lastTimeInBattle + enemybase.battleLastDuration;
    }

}
