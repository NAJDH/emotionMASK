using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNormalATK2 : playerState
{
    public AudioClip hitSound; // 命中音效
    [Header("普通攻击判定")]
    public string normalATKHitboxName = "normalATK"; // 🟢 改用字符串名称

    private PlayerHitboxManager hitboxManager; // 🟢 引用管理器

    public playerNormalATK2(player player, playerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(playerStateManager.XI)
        {
            stateTimer = 0.2f;
            //播放音效
            AudioManager.PlayAudio("attack");
        }
        else if(playerStateManager.NU)
        {
            
        }
        else if(playerStateManager.AI)
        {
            
        }
        else if(playerStateManager.JU)
        {
            stateTimer = 0.2f;
            //播放音效
            AudioManager.PlayAudio("attack");
        }
        
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


        if(playerStateManager.XI)
        {
            Debug.Log($"AnimationTriggered: {player.animEvent.AnimationTriggered}, AnimationTriggered2: {player.animEvent.AnimationTriggered2}");
            if (player.animEvent.AnimationTriggered2)
            {
            player.animEvent.AnimationTriggered2 = false;
            // 示例:在玩家攻击状态中生成子弹
            GameObject projectilePrefab = player.playerProjectilePrefab; // 假设玩家有一个投射物预制体引用
            Vector3 spawnPosition = player.transform.position + (player.isFacingRight ? Vector3.right
                : Vector3.left) * 1f +  Vector3.up * 1f; // 根据朝向调整生成位置
            Quaternion spawnRotation = Quaternion.identity;
            GameObject projectileInstance = GameObject.Instantiate(projectilePrefab, spawnPosition, spawnRotation);
            }
            
        }
        else if(playerStateManager.NU)
        {
            
        }
        else if(playerStateManager.AI)
        {
            
        }
        else if(playerStateManager.JU)
        {
            Debug.Log($"AnimationTriggered: {player.animEvent.AnimationTriggered}, AnimationTriggered2: {player.animEvent.AnimationTriggered2}");
            if (player.animEvent.AnimationTriggered2)
            {
            player.animEvent.AnimationTriggered2 = false;
            // 示例:在玩家攻击状态中生成子弹
            GameObject projectilePrefab = player.playerProjectilePrefab; // 假设玩家有一个投射物预制体引用
            Vector3 spawnPosition = player.transform.position + (player.isFacingRight ? Vector3.right
                : Vector3.left) * 1f +  Vector3.up * 1f; // 根据朝向调整生成位置
            Quaternion spawnRotation = Quaternion.identity;
            GameObject projectileInstance = GameObject.Instantiate(projectilePrefab, spawnPosition, spawnRotation);
            }
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


        if(playerStateManager.XI)
        {
        // 计算伤害（可以调用 playerStateManager 的伤害计算）
        float finalDamage = playerStateManager.playerCalculateDamage(10);
        
        // 调用敌人的受伤接口（传入2个参数）
        target.TakeDamage(finalDamage);

        // 触发击中停顿效果
        HitStopManager.Instance.TriggerHitStop(1.5f, 0.15f, "PlayerAttackHit", false);
            
        }
        else if(playerStateManager.NU)
        {
            
        }
        else if(playerStateManager.AI)
        {
            
        }
        else if(playerStateManager.JU)
        {
            
        }


    }

    public override void Exit()
    {
        if (hitboxManager != null) hitboxManager.DisableHitbox(normalATKHitboxName);
        // 重置动画事件标志，确保下次进入时能正常工作
        player.animEvent.ResetAnimationEvent();
        base.Exit();
    }
}