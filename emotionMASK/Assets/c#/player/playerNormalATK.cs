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
        Debug.Log("进入普通攻击状态");
        stateTimer = 0.3f;
        HitStopManager.Instance.TriggerHitStop(2.0f, 0.2f, "PlayerAttackHit", false);

        //播放音效（还没写）
        //攻击框（Collider2D）在动画事件里控制开关

        if (normalATKHitbox != null) normalATKHitbox.enabled = false; // 进入时确保关闭
    }

    public override void Update()
    {
        base.Update();
        //攻击的前一点点时间，让角色不完全直接停下来，优化手感
        if(stateTimer < 0)
        {
        player.SetVelocity(0f, player.rb.velocity.y);
            
        }
        // if(playerStateManager.XI)
        // {
        //     player.anim.SetInteger("whoATK", 1);
        // }
        // else if(playerStateManager.NU)
        // {
        //     player.anim.SetInteger("whoATK", 2);
        // }
        // else if(playerStateManager.AI)
        // {
        //     player.anim.SetInteger("whoATK", 3);
        // }
        // else if(playerStateManager.JU)
        // {
        //     player.anim.SetInteger("whoATK", 4);
        // }
        // 通过动画事件控制判定开关
        if (player.animEvent.hitTriggered && normalATKHitbox != null)
        {
            Debug.Log("普通攻击判定开启");
            normalATKHitbox.enabled = true;
        }
        else if (normalATKHitbox != null)
        {
            Debug.Log("普通攻击判定关闭");
            normalATKHitbox.enabled = false;
        }
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
        // 重置动画事件标志，确保下次进入时能正常工作
        player.animEvent.ResetAnimationEvent();
        base.Exit();
    }
}
