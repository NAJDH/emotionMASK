// FearEnemy.cs - ����̬����
using UnityEngine;

public class FearEnemy : Enemy, IFixedFormEnemy
{
    [Header("����̬��������")]
    [SerializeField] private float fearRadius = 3f;
    [SerializeField] private float invisibilityDuration = 4f;
    [SerializeField] private float invisibilityCooldown = 10f;
    [SerializeField] private ParticleSystem fearEffect;
    [SerializeField] private ParticleSystem invisibilityEffect;

    private bool isInvisible = false;
    private float lastInvisibilityTime;

    public MaskType FixedForm => MaskType.Fear;
    public string EnemyTypeName => "��֮����";

    protected override void Awake()
    {
        base.Awake();

        // ����̬���
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.8f, 0.6f, 1f); // ��ɫ��
        }
    }

    protected override void Update()
    {
        base.Update();

        // ������ȴ���
        if (!isInvisible && Time.time > lastInvisibilityTime + invisibilityCooldown)
        {
            TryBecomeInvisible();
        }

        // �־�⻷Ч��
        ApplyFearAura();
    }

    private void TryBecomeInvisible()
    {
        // �ڰ�ȫ����²�����
        if (!IsPlayerTooClose())
        {
            StartCoroutine(BecomeInvisible());
        }
    }

    private System.Collections.IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        lastInvisibilityTime = Time.time;

        // ������Ч
        if (invisibilityEffect != null)
            invisibilityEffect.Play();

        // ����͸����
        if (spriteRenderer != null)
        {
            Color c = spriteRenderer.color;
            c.a = 0.3f;
            spriteRenderer.color = c;
        }

        // ���ܻ�ı�layer������޷�����
        // gameObject.layer = LayerMask.NameToLayer("InvisibleEnemy");

        yield return new WaitForSeconds(invisibilityDuration);

        // �ָ��ɼ�
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
        // �������Ƿ��ڸ���
        Collider2D player = Physics2D.OverlapCircle(transform.position, fearRadius, LayerMask.GetMask("Player"));
        return player != null;
    }

    private void ApplyFearAura()
    {
        if (isInvisible) return;

        Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, fearRadius, LayerMask.GetMask("Player"));

        foreach (var player in players)
        {
            // �����ʩ�ӿ־�Ч��
            // ������Դ�����ҵĿ־�״̬����Ҫ����player�ű���ʵ�֣�
            if (fearEffect != null && !fearEffect.isPlaying)
            {
                fearEffect.transform.position = player.transform.position;
                fearEffect.Play();
            }
        }
    }

    public void OnFormAbilityTrigger()
    {
        // ����̬�����������־��Х
        Debug.Log($"{EnemyTypeName} �����־��Х��");

        // ����������������������
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

        // ����־�Ч��
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

    // ��дTakeDamage������ʱ����
    public override void TakeDamage(float amount)
    {
        if (isInvisible)
        {
            amount *= 0.5f; // ����ʱ����50%

            // �ܵ��������ܴ�������
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

        base.TakeDamage(amount);
    }
}