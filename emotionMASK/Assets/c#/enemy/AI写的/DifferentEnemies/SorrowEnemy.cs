using UnityEngine;


public class SorrowEnemy : Enemy, IFixedFormEnemy
{
    [Header("哀形态特有属性")]
    [SerializeField] private float slowFieldRadius = 4f;
    [SerializeField] private float slowEffect = 0.6f; // 减速到60%速度
    [SerializeField] private GameObject slowFieldPrefab;
    [SerializeField] private float summonInterval = 8f;

    private float lastSummonTime;
    private GameObject activeSlowField;

    public MaskType FixedForm => MaskType.Sorrow;
    public string EnemyTypeName => "哀之幽魂";

    protected override void Awake()
    {
        base.Awake();

        // 哀形态外观
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(0.7f, 0.8f, 1f); // 蓝色调
        }

        // 创建减速领域
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

        // 定时召唤
        if (Time.time > lastSummonTime + summonInterval)
        {
            TrySummonMinion();
            lastSummonTime = Time.time;
        }
    }

    private void TrySummonMinion()
    {
        // 这里可以根据需要实现召唤小怪的逻辑
        Debug.Log($"{EnemyTypeName} 尝试召唤仆从...");
        // 比如：Instantiate(minionPrefab, transform.position + Random.insideUnitSphere, Quaternion.identity);
    }

    public void OnFormAbilityTrigger()
    {
        // 哀形态特殊能力：哀伤领域
        Debug.Log($"{EnemyTypeName} 展开哀伤领域！");

        // 扩大减速领域
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

        // 恢复原大小
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

    // 减速效果应用（可以配合其他脚本使用）
    public void ApplySlowEffect(Collider2D other)
    {
        // 对进入领域的玩家或友军施加减速
        // 需要配合其他系统实现
    }
}