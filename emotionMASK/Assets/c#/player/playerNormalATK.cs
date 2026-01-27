using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNormalATK : playerState
{
    public AudioClip hitSound; // 命中音效
    [Header("普通攻击判定")]
    public Collider2D normalATKHitbox; // 在 Inspector 中拖入对应的触发器

    public playerNormalATK(player player, playerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        HitStopManager.Instance.PlayHitStop();
        player.SetVelocity(0f, player.rb.velocity.y);
        if (normalATKHitbox != null) normalATKHitbox.enabled = false; // 进入时确保关闭
    }

    public override void Update()
    {
        base.Update();

        // 通过动画事件控制判定开关
        if (player.animEvent.hitTriggered && normalATKHitbox != null)
        {
            normalATKHitbox.enabled = true;
        }
        else if (normalATKHitbox != null)
        {
            normalATKHitbox.enabled = false;
        }

        // 攻击结束后返回待机状态
        if (player.animEvent.AnimationTriggered)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        if (normalATKHitbox != null) normalATKHitbox.enabled = false;
        base.Exit();
    }
}
