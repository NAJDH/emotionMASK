using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    private float customIdleTime = -1f;
    // 每隔多少秒左右看一次
    private float lookInterval = 1f;
    private float lookTimer;

    public Enemy_IdleState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName) : base(enemybase, stateMachine, animBoolName)
    {
    }

    public void SetCustomIdleTime(float time)
    {
        customIdleTime = time;
    }

    public override void Enter()
    {
        base.Enter();

        enemybase.SetVelocity(0, enemybase.rb.velocity.y);

        if (customIdleTime > 0f)
        {
            stateTimer = customIdleTime;
            customIdleTime = -1f; // 用完即清
        }
        else
        {
            stateTimer = enemybase.idleTime;
        }

        // 初始化左右看计时器
        lookTimer = lookInterval;
    }

    public override void Update()
    {
        base.Update();

        // base.Update 可能在检测到玩家时已经切状态，若不是当前状态则不再继续执行本状态逻辑
        if (stateMachine.currentState != this) return;

        // 失去视野的空闲状态中每隔 lookInterval 翻转一次
        lookTimer -= Time.deltaTime;
        if (lookTimer <= 0f)
        {
            enemybase.Flip();
            lookTimer = lookInterval;
        }

        if (stateTimer < 0)
            stateMachine.ChangeState(enemybase.moveState);
    }
}
