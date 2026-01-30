// JoyEnemy.cs - 喜形态敌人
using UnityEngine;

public class JoyEnemy : Enemy, IFixedFormEnemy
{
    [Header("喜形态特有属性")]
    [SerializeField] private float healAmount = 5f;
    [SerializeField] private float healRadius = 3f;
    [SerializeField] private float healInterval = 3f;
    //[SerializeField] private ParticleSystem healEffect;

    private float lastHealTime;
    private Color originalColor;

    public MaskType FixedForm => MaskType.Joy;
    public string EnemyTypeName => "喜之妖灵";

    protected override void Awake()
    {
        base.Awake();

        // 喜形态特有的初始化
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1f, 0.95f, 0.8f); // 浅金色
        }
    }

    protected override void Start()
    {
        base.Start();
        lastHealTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();

        // 定时治疗周围敌人
        if (Time.time > lastHealTime + healInterval)
        {
            TryHealNearbyEnemies();
            lastHealTime = Time.time;
        }
    }

    //可选的特殊效果
    private void TryHealNearbyEnemies()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, healRadius);
        bool healedAny = false;

        foreach (var col in nearby)
        {
            Enemy otherEnemy = col.GetComponent<Enemy>();
            if (otherEnemy != null && otherEnemy != this)
            {
                // 这里需要添加治疗逻辑，你可以根据自己的系统实现
                healedAny = true;
            }
        }

        //if (healedAny && healEffect != null)
        //{
        //    healEffect.Play();
        //}
    }

    public void OnFormAbilityTrigger()
    {
        // 喜形态特殊能力：鼓舞光环
        Debug.Log($"{EnemyTypeName} 释放鼓舞光环！");

        //if (healEffect != null)
        //{
        //    healEffect.transform.localScale = Vector3.one * healRadius * 2;
        //    healEffect.Play();
        //}
    }

    // 重写 TakeDamage，喜形态对哀形态有抗性
    public new void TakeDamage(float amount, MaskType attackerMask)
    {
        // 计算克制关系
        if (attackerMask == MaskType.Sorrow)
        {
            amount *= 0.7f; // 对哀形态攻击有30%减伤
        }

        base.TakeDamage(amount, attackerMask);
    }
}