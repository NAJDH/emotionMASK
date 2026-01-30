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

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        IDamageable target = collision.GetComponent<IDamageable>();

        // 2. 只有当目标有效，且不在“已打中名单”里时
        if (target != null && !_hitList.Contains(target))
        {
            //加入白名单，保证同一个攻击框只打中一次
            _hitList.Add(target);

            // 3. 【核心】直接告诉玩家：“我打中这个家伙了，剩下的你看着办！”
            if (_ownerPlayer != null)
            {
                _ownerPlayer.OnAttackHit(target, collision);
            }
        }
        else
        {
            Debug.Log("❌ 没有找到 IDamageable 接口或已在列表中"); // ← 添加这行
        }
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