// FearEnemy.cs - 惧形态敌人
using UnityEngine;

public class FearEnemy : Enemy, IFixedFormEnemy
{
    [Header("惧形态特有属性")]
    [SerializeField] private float fearRadius = 3f;
    [SerializeField] private float invisibilityDuration = 4f;
    [SerializeField] private float invisibilityCooldown = 10f;
    [SerializeField] private ParticleSystem fearEffect;
    [SerializeField] private ParticleSystem invisibilityEffect;

    private bool isInvisible = false;
    private float lastInvisibilityTime;

    public MaskType FixedForm => MaskType.Fear;
    public string EnemyTypeName => "惧之梦魇";

    protected override void Awake()
    {
        base.Awake();

        // 惧形态外观
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, 0.6f, 1f); // 紫色调
        }
    }

    protected override void Update()
    {
        base.Update();

        // 隐身冷却检查
        if (!isInvisible && Time.time > lastInvisibilityTime + invisibilityCooldown)
        {
            TryBecomeInvisible();
        }

        // 恐惧光环效果
        ApplyFearAura();
    }

    private void TryBecomeInvisible()
    {
        // 在安全情况下才隐身
        if (!IsPlayerTooClose())
        {
            StartCoroutine(BecomeInvisible());
        }
    }

    private System.Collections.IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        lastInvisibilityTime = Time.time;

        // 隐身特效
        if (invisibilityEffect != null)
            invisibilityEffect.Play();

        // 降低透明度
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.3f;
            spriteRenderer.color = c;
        }

        // 可能会改变layer让玩家无法锁定
        // gameObject.layer = LayerMask.NameToLayer("InvisibleEnemy");

        yield return new WaitForSeconds(invisibilityDuration);

        // 恢复可见
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 1f;
            spriteRenderer.color = c;
        }

        // gameObject.layer = LayerMask.NameToLayer("Enemy");

        isInvisible = false;
    }

    private bool IsPlayerTooClose()
    {
        // 检测玩家是否在附近
        Collider2D player = Physics2D.OverlapCircle(transform.position, fearRadius, LayerMask.GetMask("Player"));
        return player != null;
    }

    private void ApplyFearAura()
    {
        if (isInvisible) return;

        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, fearRadius, LayerMask.GetMask("Player"));

        foreach (var player in players)
        {
            // 对玩家施加恐惧效果
            // 这里可以触发玩家的恐惧状态（需要你在player脚本中实现）
            if (fearEffect != null && !fearEffect.isPlaying)
            {
                fearEffect.transform.position = player.transform.position;
                fearEffect.Play();
            }
        }
    }

    public void OnFormAbilityTrigger()
    {
        // 惧形态特殊能力：恐惧尖啸
        Debug.Log($"{EnemyTypeName} 发出恐惧尖啸！");

        // 打断隐身（如果正在隐身）
        if (isInvisible)
        {
            StopAllCoroutines();
            if (spriteRenderer != null)
            {
                Color c = spriteRenderer.color;
                c.a = 1f;
                spriteRenderer.color = c;
            }
            isInvisible = false;
        }

        // 扩大恐惧效果
        StartCoroutine(FearScream());
    }

    private System.Collections.IEnumerator FearScream()
    {
        float originalRadius = fearRadius;
        fearRadius *= 2f;

        if (fearEffect != null)
        {
            fearEffect.transform.localScale = Vector3.one * fearRadius;
            fearEffect.Play();
        }

        yield return new WaitForSeconds(2f);

        fearRadius = originalRadius;
    }

    // 重写TakeDamage，隐身时减伤
    public override void TakeDamage(float amount, MaskType attackerMask)
    {
        if (isInvisible)
        {
            amount *= 0.5f; // 隐身时减伤50%

            // 受到攻击可能打破隐身
            if (Random.value > 0.5f)
            {
                StopAllCoroutines();
                if (spriteRenderer != null)
                {
                    Color c = spriteRenderer.color;
                    c.a = 1f;
                    spriteRenderer.color = c;
                }
                isInvisible = false;
            }
        }

        base.TakeDamage(amount, attackerMask);
    }
}