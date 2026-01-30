using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    // 持有玩家的引用，为了能通知回去
    private player _ownerPlayer;
    
    // 记录本次开启期间打中的敌人
    private List<IDamageable> _hitList = new List<IDamageable>();

    private void Awake()
    {
        // 自动向上寻找 Player 组件
        _ownerPlayer = GetComponentInParent<player>();
    }

    private void OnEnable()
    {
        // 每次攻击框开启（激活）时，清空受击名单
        _hitList.Clear();
        Debug.Log($"🟢 攻击框已激活：{gameObject.name}");
        
        // 检查组件
        Collider2D col = GetComponent<Collider2D>();
        if (col == null)
        {
            Debug.LogError("❌ 攻击框缺少 Collider2D 组件！");
        }
        else
        {
            Debug.Log($"✅ Collider2D 存在，Is Trigger = {col.isTrigger}, Enabled = {col.enabled}");
        }
    }

    private void Update()
    {
        // 实时显示攻击框是否激活
        Debug.Log($"⏰ 攻击框 {gameObject.name} 正在运行，激活状态：{gameObject.activeSelf}");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 检查是否触发了物理碰撞
    Debug.Log($"攻击框碰到了: {collision.name}, Tag是: {collision.tag}");

    if(collision.CompareTag("Enemy"))
    {
        // 2. 检查是否找到了受伤接口
        var damageable = collision .GetComponent<IDamageable>(); // 或者 GetComponentInParent
        if (damageable != null)
        {
            Debug.Log("找到 IDamageable，尝试造成伤害");
            damageable.TakeDamage(10f); // 传递伤害值和当前形态
        }
        else
        {
            Debug.LogError("碰到了 Enemy，但它身上没有 IDamageable (比如 Enemy 脚本)！");
        }
    }
        // Debug.Log($"🎯 攻击框触碰到：{collision.gameObject.name}"); // ← 添加这行
        
        // IDamageable target = collision.GetComponent<IDamageable>();

        // // 2. 只有当目标有效，且不在“已打中名单”里时
        // if (target != null && !_hitList.Contains(target))
        // {
        //     //加入白名单，保证同一个攻击框只打中一次
        //     Debug.Log("✅ 找到 IDamageable 接口！"); // ← 添加这行
        //     _hitList.Add(target);

        //     // 3. 【核心】直接告诉玩家：“我打中这个家伙了，剩下的你看着办！”
        //     if (_ownerPlayer != null)
        //     {
        //         Debug.Log("✅ 通知玩家攻击命中！"); // ← 添加这行
        //         _ownerPlayer.OnAttackHit(target, collision);
        //     }
        // }
        // else
        // {
        //     Debug.Log("❌ 没有找到 IDamageable 接口或已在列表中"); // ← 添加这行
        // }
    }
}


// using UnityEngine;

// public class PlayerHitbox : MonoBehaviour
// {
//     public float damage = 10f;
//     [Header("命中特效")]
//     public GameObject hitEffectPrefab; // 命中特效预制体

//     // 标记是否已经命中敌人，防止多次触发，以及备用
//     public bool hasHitEnemy = false;

//     private void OnEnable()
//     {
//         //初始化
//         hasHitEnemy = false;
//     }

//     private void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (hasHitEnemy) return; // 已经命中过敌人，直接返回

//         IDamageable target = collision.GetComponent<IDamageable>();
//         if(target != null)
//         {

//             //这里写打中的逻辑
//             // target.TakeDamage(damage);
//             //播放打中声音
            

//             hasHitEnemy = true; // 标记为已命中
//         }
//         if(target != null && hitEffectPrefab != null)
//         {
//             //生成命中特效
//             //collision.ClosestPoint(transform.position) 可以获取碰撞点
//             Instantiate(hitEffectPrefab, collision.ClosestPoint(transform.position), Quaternion.identity);
//         }
//     }


//     // private void OnTriggerEnter2D(Collider2D collision)
//     // {
//     //     Debug.Log("收到攻击 " + collision.name);
//     //     Enemy enemy = collision.GetComponent<Enemy>();
//     //     if (enemy != null)
//     //     {
//     //         enemy.Damage();
//     //     }
//     // }
// }