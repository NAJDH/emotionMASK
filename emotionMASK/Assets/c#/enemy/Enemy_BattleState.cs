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

        var hit = enemybase.PlayerDetected();
        if (hit.collider != null)
            player = hit.transform;
        else
            player = null;

        if(ShouldRetreat())
        {
            enemybase.rb.velocity = new Vector2(enemybase.retreatVelocity.x * FacingDirectionToPlayer(), enemybase.retreatVelocity.y);
            enemybase.FilpController(FacingDirectionToPlayer());
        }
    }

    public override void Update()
    {
        base.Update();

        var hit = enemybase.PlayerDetected();
        if (hit.collider != null)
        {
            player = hit.transform;
            UpdateLastBattleTime();
        }
        else
        {
            // 立刻进入 idle，停 5 秒
            enemybase.idleState.SetCustomIdleTime(5f);
            stateMachine.ChangeState(enemybase.idleState);
            return;
        }

        if (WithinTheAttackDistance() && player != null)
            stateMachine.ChangeState(enemybase.attackState);
        else
        {
            int dir = FacingDirectionToPlayer();
            if (dir != 0)
            {
                enemybase.SetVelocity(enemybase.battleMoveSpeed * dir, enemybase.rb.velocity.y);
                enemybase.FilpController(dir);
            }
            else
            {
                enemybase.SetZeroVelocity();
            }
        }
    }

    private bool WithinTheAttackDistance()
    {
        return DistanceToPlayer() <= enemybase.attackDistance;
    }

    private float DistanceToPlayer()
    {
        if (player == null)
            return float.MaxValue;

        return Mathf.Abs(player.position.x - enemybase.transform.position.x);
    }

    private int FacingDirectionToPlayer()
    {
        // ���û����֪ player������ 0�����ƶ���
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
