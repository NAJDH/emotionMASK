// JoyEnemy.cs - ϲ��̬����
using UnityEngine;

public class JoyEnemy : Enemy, IFixedFormEnemy
{
    [Header("ϲ��̬��������")]
    [SerializeField] private float healAmount = 5f;
    [SerializeField] private float healRadius = 3f;
    [SerializeField] private float healInterval = 3f;
    //[SerializeField] private ParticleSystem healEffect;

    private float lastHealTime;
    private Color originalColor;

    public MaskType FixedForm => MaskType.Joy;
    public string EnemyTypeName => "ϲ֮����";

    protected override void Awake()
    {
        base.Awake();

        // ϲ��̬���еĳ�ʼ��
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(1f, 0.95f, 0.8f); // ǳ��ɫ
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

        // ��ʱ������Χ����
        if (Time.time > lastHealTime + healInterval)
        {
            TryHealNearbyEnemies();
            lastHealTime = Time.time;
        }
    }

    //��ѡ������Ч��
    private void TryHealNearbyEnemies()
    {
        Collider2D[] nearby = Physics2D.OverlapCircleAll(transform.position, healRadius);
        bool healedAny = false;

        foreach (var col in nearby)
        {
            Enemy otherEnemy = col.GetComponent<Enemy>();
            if (otherEnemy != null && otherEnemy != this)
            {
                // ������Ҫ���������߼�������Ը����Լ���ϵͳʵ��
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
        // ϲ��̬��������������⻷
        Debug.Log($"{EnemyTypeName} �ͷŹ���⻷��");

        //if (healEffect != null)
        //{
        //    healEffect.transform.localScale = Vector3.one * healRadius * 2;
        //    healEffect.Play();
        //}
    }

    // ��д TakeDamage��ϲ��̬�԰���̬�п���
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }
}