using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    public Animator animator; // 在 Inspector 挂入，用于播放 Victory/Failure 动画
    public float outcomeAnimationLength = 2.0f; // 备用：如果没有动画事件可用这个时长

    [Header("敌人生成")]
    public GameObject enemyJoyPrefab;
    public GameObject enemyAngerPrefab;
    public GameObject enemySorrowPrefab;
    public GameObject enemyFearPrefab;

    [Tooltip("可选：用于放置生成点，优先使用这些点。若为空则在中心附近随机生成")]
    public Transform[] spawnPoints;

    [Tooltip("实例化的敌人将作为该父对象的子对象，若为空则自动创建")]
    public Transform enemyParent;

    [Tooltip("生成敌人数量范围（包含 min 和 max）")]
    public int minEnemies = 4;
    public int maxEnemies = 5;

    [Tooltip("进入战斗后冻结时长（秒，真实时间，不受 Time.timeScale 影响）")]
    public float freezeDuration = 2f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        // 启动：先生成敌人并冻结两秒，再让战斗正常进行
        StartCoroutine(SpawnAndFreezeCoroutine());
    }

    private IEnumerator SpawnAndFreezeCoroutine()
    {
        // 确保 parent 存在
        if (enemyParent == null)
        {
            var go = new GameObject("Enemies");
            DontDestroyOnLoad(go); // 可选：如果你想保留跨场景则保留，否则注释掉
            enemyParent = go.transform;
        }

        SpawnEnemies();

        // 时间冻结（真实时间等待）
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = 1f;

        // 冻结结束后可以做额外初始化（例如启用 AI、HUD 提示等）
        yield break;
    }

    private void SpawnEnemies()
    {
        // 选择生成数量（包含）
        int count = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = ChooseSpawnPosition(i, count);
            GameObject prefab = ChooseRandomEnemyPrefab();
            if (prefab == null)
            {
                Debug.LogWarning("BattleSceneController: 某个敌人预制体为空，请在 Inspector 中设置。");
                continue;
            }

            GameObject inst = Instantiate(prefab, pos, Quaternion.identity, enemyParent);
            spawnedEnemies.Add(inst);
        }
    }

    private Vector3 ChooseSpawnPosition(int index, int total)
    {
        // 优先使用配置的 spawnPoints
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // 如果 spawnPoints 少于生成数量，循环使用或随机抽取
            int choose = spawnPoints.Length >= total ? index % spawnPoints.Length : Random.Range(0, spawnPoints.Length);
            return spawnPoints[choose].position;
        }

        // 否则在场景中心附近随机生成位置（X 轴分散，Y 保持与本对象相同）
        float spread = 3.0f + total * 0.5f;
        float x = transform.position.x + Random.Range(-spread, spread);
        float y = transform.position.y;
        return new Vector3(x, y, 0f);
    }

    private GameObject ChooseRandomEnemyPrefab()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0: return enemyJoyPrefab;
            case 1: return enemyAngerPrefab;
            case 2: return enemySorrowPrefab;
            default: return enemyFearPrefab;
        }
    }

    // 原有接口：由具体战斗逻辑在战斗结束时调用（胜利或失败）
    public void OnBattleEnded(bool victory)
    {
        // 告诉 CheckpointManager 战斗结果，Manager 会调用本类的 PlayOutcomeAnimation（若找到）
        CheckpointManager.ReportBattleResult(victory);
    }

    // CheckpointManager 可能会直接调用此方法来播放对应的动画
    public void PlayOutcomeAnimation(bool victory)
    {
        if (animator != null)
        {
            animator.SetTrigger(victory ? "Victory" : "Failure");
            // 如果你有动画事件可以直接调用 CheckpointManager.NotifyBattleAnimationComplete，
            // 这里用协程等待一个固定时长再通知（便于演示）
            StartCoroutine(WaitAndNotify(victory));
        }
        else
        {
            // 如果没有 animator，直接通知（避免卡住）
            CheckpointManager.NotifyBattleAnimationComplete(victory);
        }
    }

    private IEnumerator WaitAndNotify(bool victory)
    {
        // 使用真实时间等待动画结束（动画可能依赖 Time.timeScale; 这里用 unscaled）
        yield return new WaitForSecondsRealtime(outcomeAnimationLength);
        CheckpointManager.NotifyBattleAnimationComplete(victory);
    }

    // 可选：提供外部清理函数（当你需要在战斗结束后销毁残留敌人时调用）
    public void CleanupSpawnedEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            var go = spawnedEnemies[i];
            if (go != null) Destroy(go);
            spawnedEnemies.RemoveAt(i);
        }
    }
}