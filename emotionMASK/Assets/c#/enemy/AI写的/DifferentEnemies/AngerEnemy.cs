// AngerEnemy.cs - ŭ��̬����
using UnityEngine;

public class AngerEnemy : Enemy, IFixedFormEnemy
{
    [Header("ŭ��̬��������")]
    [SerializeField] private float rageDamageMultiplier = 1.5f;
    [SerializeField] private float rageSpeedMultiplier = 1.3f;
    [SerializeField] private float rageThreshold = 0.5f; // Ѫ������50%���뱩ŭ
    [SerializeField] private ParticleSystem rageEffect;

    private bool isRaging = false;

    public MaskType FixedForm => MaskType.Anger;
    public string EnemyTypeName => "ŭ֮ս��";

    protected override void Awake()
    {
        base.Awake();

        // ŭ��̬���
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(1f, 0.5f, 0.5f); // ��ɫ��
        }
    }

    protected override void Update()
    {
        base.Update();

        // ����Ƿ�Ӧ�ý��뱩ŭ״̬
        if (!isRaging && currentHealth / maxHealth < rageThreshold)
        {
            EnterRageMode();
        }

        // ��ŭ״̬�µĶ�����Ϊ
        if (isRaging)
        {
            // ���ܻ����Ӷ���Ĺ�����������Ϊ
        }
    }

    private void EnterRageMode()
    {
        isRaging = true;

        // ��ǿ����
        moveSpeed *= rageSpeedMultiplier;
        // attackDamage �����߼������Enemy����attackDamage���ԣ�

        // ��ŭ��Ч
        if (rageEffect != null)
        {
            rageEffect.Play();
        }

        // ��������
        anim.SetBool("IsRaging", true);
        Debug.Log($"{EnemyTypeName} ���뱩ŭ״̬��");
    }

    public void OnFormAbilityTrigger()
    {
        // ŭ��̬�����������񱩳��
        Debug.Log($"{EnemyTypeName} �����񱩳�棡");

        // ��ǰ���
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

    // ��дTakeDamage��ŭ��̬��ϲ��̬�ж����˺�
    public new void TakeDamage(float amount)
    {

        base.TakeDamage(amount);
    }
}