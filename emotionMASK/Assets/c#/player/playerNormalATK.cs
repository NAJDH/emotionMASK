using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNormalATK : playerState
{
    public AudioClip hitSound; // 命中音效
    [Header("普通攻击判定")]
    public string normalATKHitboxName = "normalATK"; // 🟢 改用字符串名称

    private PlayerHitboxManager hitboxManager; // 🟢 引用管理器

    public playerNormalATK(player player, playerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.3f;
        HitStopManager.Instance.TriggerHitStop(2.0f, 0.2f, "PlayerAttackHit", false);

        //播放音效（还没写）
        //攻击框（Collider2D）在动画事件里控制开关
        hitboxManager = player.GetComponent<PlayerHitboxManager>(); // 获取管理器引用
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
        // 🟢 使用 PlayerHitboxManager 来控制判定开关
        if (player.animEvent.hitTriggered && hitboxManager != null)
        {
            Debug.Log("普通攻击判定开启");
            hitboxManager.EnableHitbox(normalATKHitboxName); // ← 使用管理器开启
        }
        else if (!player.animEvent.hitTriggered && hitboxManager != null)
        {
            Debug.Log("普通攻击判定关闭");
            hitboxManager.DisableHitbox(normalATKHitboxName); // ← 使用管理器关闭
        }
        // 攻击结束后返回待机状态
        if (player.animEvent.AnimationTriggered)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    // 🟢 关键：重写这个方法来处理命中逻辑
    public override void OnAttackHit(IDamageable target, Collider2D hitInfo)
    {
        Debug.Log("🔥 普通攻击命中敌人！！！！！！！！！！！");
        
        // 计算伤害（可以调用 playerStateManager 的伤害计算）
        float finalDamage = playerStateManager.playerCalculateDamage(10);
        
        // 获取当前形态作为攻击者的面具类型
        MaskType currentMask = (MaskType)(PlayerFormManager.playerForm.currentFormIndex - 1);
        
        // 调用敌人的受伤接口（传入2个参数）
        target.TakeDamage(finalDamage);
        
        // 播放音效
        // if (hitSound != null)
        // {
        //     AudioManager.Instance.Play(hitSound);
        // }
        
        // 生成特效（如果需要）
        // Instantiate(hitEffectPrefab, hitInfo.ClosestPoint(player.transform.position), Quaternion.identity);
    }

    public override void Exit()
    {
        if (hitboxManager != null) hitboxManager.DisableHitbox(normalATKHitboxName);
        // 重置动画事件标志，确保下次进入时能正常工作
        player.animEvent.ResetAnimationEvent();
        base.Exit();
    }
}