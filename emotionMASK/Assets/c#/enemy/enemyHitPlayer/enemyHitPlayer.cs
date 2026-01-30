using UnityEngine;

public class EnemyHitbox : MonoBehaviour
{
    [Header("伤害设置")]
    public int damageAmount = 10; // 敌人造成的伤害值

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 检查是不是碰到了玩家
        // ⚠️ 确保你的玩家物体 Tag 设置为 "Player"
        Debug.Log("敌人攻击判定触发！！！！！！！！！！！！！！！");
        if (other.CompareTag("Player"))
        {
            playerStateManager.enemyHitPlayerDamage(damageAmount);
            Debug.Log("敌人命中玩家，造成 " + damageAmount + " 点伤害！！！！！！！！！！！！！！！");
        }
    }
}