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
        if(playerStateManager.XI)
        {
            player.anim.SetInteger("whoATK", 1);
        }
        else if(playerStateManager.NU)
        {
            player.anim.SetInteger("whoATK", 2);
        }
        else if(playerStateManager.AI)
        {
            player.anim.SetInteger("whoATK", 3);
        }
        else if(playerStateManager.JU)
        {
            player.anim.SetInteger("whoATK", 4);
        }
        // 通过动画事件控制判定开关
        if (player.animEvent.hitTriggered && normalATKHitbox != null)
        {
            normalATKHitbox.enabled = true;
        }
        else if (normalATKHitbox != null)
        {
            normalATKHitbox.enabled = false;
        }
        Debug.Log("普通攻击状态更新");
        // 攻击结束后返回待机状态
        if (player.animEvent.AnimationTriggered)
        {
            Debug.Log("普通攻击动画结束，返回待机状态");
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        Debug.Log("退出普通攻击状态");
        if (normalATKHitbox != null) normalATKHitbox.enabled = false;
        base.Exit();
    }
}
