using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public float damage = 10f;
    [Header("命中特效")]
    public GameObject hitEffectPrefab; // 命中特效预制体

    // 标记是否已经命中敌人，防止多次触发，以及备用
    public bool hasHitEnemy = false;

    private void OnEnable()
    {
        //初始化
        hasHitEnemy = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHitEnemy) return; // 已经命中过敌人，直接返回

        IDamageable target = collision.GetComponent<IDamageable>();
        if(target != null)
        {

            //这里写打中的逻辑
            // target.TakeDamage(damage);
            //播放打中声音
            

            hasHitEnemy = true; // 标记为已命中
        }
        if(target != null && hitEffectPrefab != null)
        {
            //生成命中特效
            //collision.ClosestPoint(transform.position) 可以获取碰撞点
            Instantiate(hitEffectPrefab, collision.ClosestPoint(transform.position), Quaternion.identity);
        }
    }


    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     Debug.Log("收到攻击 " + collision.name);
    //     Enemy enemy = collision.GetComponent<Enemy>();
    //     if (enemy != null)
    //     {
    //         enemy.Damage();
    //     }
    // }
}