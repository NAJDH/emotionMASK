using UnityEngine;


public class SorrowEnemy : Enemy, IFixedFormEnemy
{
    [Header("����̬��������")]
    [SerializeField] private float slowFieldRadius = 4f;
    [SerializeField] private float slowEffect = 0.6f; // ���ٵ�60%�ٶ�
    [SerializeField] private GameObject slowFieldPrefab;
    [SerializeField] private float summonInterval = 8f;

    private float lastSummonTime;
    private GameObject activeSlowField;

    public MaskType FixedForm => MaskType.Sorrow;
    public string EnemyTypeName => "��֮�Ļ�";

    protected override void Awake()
    {
        base.Awake();

        // ����̬���
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.7f, 0.8f, 1f); // ��ɫ��
        }

        // ������������
        if (slowFieldPrefab != null)
        {
            activeSlowField = Instantiate(slowFieldPrefab, transform);
            activeSlowField.transform.localScale = Vector3.one * slowFieldRadius * 2;
        }
    }

    protected override void Start()
    {
        base.Start();
        lastSummonTime = Time.time;
    }

    protected override void Update()
    {
        base.Update();

        // ��ʱ�ٻ�
        if (Time.time > lastSummonTime + summonInterval)
        {
            TrySummonMinion();
            lastSummonTime = Time.time;
        }
    }

    private void TrySummonMinion()
    {
        // ������Ը�����Ҫʵ���ٻ�С�ֵ��߼�
        Debug.Log($"{EnemyTypeName} �����ٻ��ʹ�...");
        // ���磺Instantiate(minionPrefab, transform.position + Random.insideUnitSphere, Quaternion.identity);
    }

    public void OnFormAbilityTrigger()
    {
        // ����̬������������������
        Debug.Log($"{EnemyTypeName} չ����������");

        // �����������
        if (activeSlowField != null)
        {
            StartCoroutine(ExpandSlowField());
        }
    }

    private System.Collections.IEnumerator ExpandSlowField()
    {
        float originalSize = activeSlowField.transform.localScale.x;
        float targetSize = originalSize * 1.5f;
        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newSize = Mathf.Lerp(originalSize, targetSize, timer / duration);
            activeSlowField.transform.localScale = Vector3.one * newSize;
            yield return null;
        }

        // �ָ�ԭ��С
        yield return new WaitForSeconds(3f);

        timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newSize = Mathf.Lerp(targetSize, originalSize, timer / duration);
            activeSlowField.transform.localScale = Vector3.one * newSize;
            yield return null;
        }
    }

    // ����Ч��Ӧ�ã�������������ű�ʹ�ã�
    public void ApplySlowEffect(Collider2D other)
    {
        // �Խ����������һ��Ѿ�ʩ�Ӽ���
        // ��Ҫ�������ϵͳʵ��
    }
}