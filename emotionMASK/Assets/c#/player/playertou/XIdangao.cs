using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XIdangao : MonoBehaviour
{
    [Header("子弹属性")]
    public float damage = 15f; // 子弹伤害
    public float bulletSpeed = 10f; // 子弹速度
    
    [Header("特效")]
    public GameObject hitEffectPrefab; // 命中特效
    public GameObject destroyEffectPrefab; // 销毁特效(落地用)
    private Rigidbody2D rb;
    private bool hasHit = false; // 防止重复触发
    private player ownerPlayer; // 玩家引用,用于获取伤害计算

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // 尝试获取玩家引用(如果需要用玩家的伤害计算)
        ownerPlayer = FindObjectOfType<player>();
    }

    private void Start()
    {
        // 设定子弹的初始速度(朝向玩家面朝的方向)
        if (rb != null)
        {
            // 根据玩家朝向决定子弹方向
            float direction = ownerPlayer != null && ownerPlayer.isFacingRight ? 1 : -1;
            rb.velocity = new Vector2(bulletSpeed * direction, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return; // 已经命中,防止重复触发

        // 检测是否击中敌人
        IDamageable target = collision.GetComponent<IDamageable>();
        if (target != null)
        {
            HitEnemy(target, collision);
            return;
        }

        // 检测是否击中地面/墙壁
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || 
            collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            HitGround(collision);
            return;
        }
    }

    /// <summary>
    /// 击中敌人的处理
    /// </summary>
    private void HitEnemy(IDamageable target, Collider2D collision)
    {
        hasHit = true;

        // 计算最终伤害(可以从玩家状态管理器获取)
        float finalDamage = damage;
        finalDamage = playerStateManager.playerCalculateDamage(damage);

        // 造成伤害
        target.TakeDamage(finalDamage);
        
        Debug.Log($"子弹击中敌人! 造成 {finalDamage} 点伤害");

        // 播放命中音效
        

        // 生成命中特效
        if (hitEffectPrefab != null)
        {
            Vector2 hitPoint = collision.ClosestPoint(transform.position);
            Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
        }

        // 触发击中停顿效果(可选)
        if (HitStopManager.Instance != null)
        {
            HitStopManager.Instance.TriggerHitStop(1f, 0.1f, "BulletHit", false);
        }

        // 销毁子弹
        Destroy(gameObject);
    }

    /// <summary>
    /// 击中地面/墙壁的处理
    /// </summary>
    private void HitGround(Collider2D collision)
    {
        hasHit = true;

        Debug.Log("子弹击中地面,销毁");

        // 播放销毁音效
        

        // 生成销毁特效
        if (destroyEffectPrefab != null)
        {
            Vector2 hitPoint = collision.ClosestPoint(transform.position);
            Instantiate(destroyEffectPrefab, hitPoint, Quaternion.identity);
        }

        // 销毁子弹
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 如果用的是 Collision2D 而不是 Trigger
        // 可以把上面 OnTriggerEnter2D 的逻辑复制到这里
        // 或者确保子弹的 Collider2D 勾选了 "Is Trigger"
        
        if (hasHit) return;

        IDamageable target = collision.gameObject.GetComponent<IDamageable>();
        if (target != null)
        {
            HitEnemy(target, collision.collider);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || 
                 collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            HitGround(collision.collider);
        }
    }
}