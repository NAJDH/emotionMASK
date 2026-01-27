// AngerEnemy.cs - 怒形态敌人
using UnityEngine;

public class AngerEnemy : Enemy, IFixedFormEnemy
{
    [Header("怒形态特有属性")]
    [SerializeField] private float rageDamageMultiplier = 1.5f;
    [SerializeField] private float rageSpeedMultiplier = 1.3f;
    [SerializeField] private float rageThreshold = 0.5f; // 血量低于50%进入暴怒
    [SerializeField] private ParticleSystem rageEffect;

    private bool isRaging = false;

    public MaskType FixedForm => MaskType.Anger;
    public string EnemyTypeName => "怒之战鬼";

    protected override void Awake()
    {
        base.Awake();

        // 怒形态外观
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f); // 红色调
        }
    }

    protected override void Update()
    {
        base.Update();

        // 检查是否应该进入暴怒状态
        if (!isRaging && currentHealth / maxHealth < rageThreshold)
        {
            EnterRageMode();
        }

        // 暴怒状态下的额外行为
        if (isRaging)
        {
            // 可能会添加额外的攻击欲望或行为
        }
    }

    private void EnterRageMode()
    {
        isRaging = true;

        // 增强属性
        moveSpeed *= rageSpeedMultiplier;
        // attackDamage 增加逻辑（如果Enemy类有attackDamage属性）

        // 暴怒特效
        if (rageEffect != null)
        {
            rageEffect.Play();
        }

        // 动画表现
        anim.SetBool("IsRaging", true);
        Debug.Log($"{EnemyTypeName} 进入暴怒状态！");
    }

    public void OnFormAbilityTrigger()
    {
        // 怒形态特殊能力：狂暴冲锋
        Debug.Log($"{EnemyTypeName} 发动狂暴冲锋！");

        // 向前冲锋
        StartCoroutine(ChargeAttack());
    }

    private System.Collections.IEnumerator ChargeAttack()
    {
        float chargeDuration = 0.8f;
        float timer = 0f;
        float chargeSpeed = moveSpeed * 2f;

        Vector2 chargeDirection = isFacingRight ? Vector2.right : Vector2.left;

        while (timer < chargeDuration)
        {
            timer += Time.deltaTime;
            rb.velocity = new Vector2(chargeDirection.x * chargeSpeed, rb.velocity.y);
            yield return null;
        }
    }

    // 重写TakeDamage，怒形态对喜形态有额外伤害
    public new void TakeDamage(float amount, MaskType attackerMask)
    {
        // 计算克制关系
        if (attackerMask == MaskType.Joy && isRaging)
        {
            // 暴怒时对喜形态攻击有特殊反应
            // 可能会触发反击或其他效果
        }

        base.TakeDamage(amount, attackerMask);
    }
}