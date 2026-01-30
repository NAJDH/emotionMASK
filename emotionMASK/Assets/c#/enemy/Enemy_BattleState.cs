using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy_BattleState : EnemyState
{
    public Transform player; // 最近一次已知的玩家 Transform（有效且活跃的形态或最后一次命中的形态）
    private float lastTimeInBattle;

    public Enemy_BattleState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 尝试获取当前可见的玩家目标（如果射线能命中）
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

        // 每帧尝试用射线检测更新玩家（优先使用当前可见的形态）
        var hit = enemybase.PlayerDetected();
        if (hit.collider != null)
        {
            // 如果射线命中一个玩家形态，立即更新为该 Transform，并刷新战斗计时
            player = hit.transform;
            UpdateLastBattleTime();
        }
        else
        {
            // 未命中时不要盲目清空 player：
            // - 如果 player 引用存在且对应的 GameObject 仍处于 active，则保留（这样当玩家绕到背后仍会转身追击）
            // - 只有当 player 被禁用或销毁时才清空引用（例如玩家旧形态被 SetActive(false)）
            if (player != null)
            {
                if (!player.gameObject.activeInHierarchy)
                {
                    player = null;
                }
                // 否则保留 player（即使当前射线没命中，也可基于 last-known 继续追击/转身）
            }
        }

        if (BattleTimeOut())
            stateMachine.ChangeState(enemybase.idleState);

        if (WithinTheAttackDistance() && player != null)
            stateMachine.ChangeState(enemybase.attackState);
        else
        {
            // 基于 player（最近一次已知位置）移动和朝向
            int dir = FacingDirectionToPlayer();
            if (dir != 0)
            {
                enemybase.SetVelocity(enemybase.battleMoveSpeed * dir, enemybase.rb.velocity.y);
                enemybase.FilpController(dir);
            }
            else
            {
                enemybase.Flip();

                // 没有有效目标则停止移动
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
        // 如果没有已知 player，返回 0（不移动）
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
