using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    public Animator animator; // �� Inspector ���룬���ڲ��� Victory/Failure ����
    public float outcomeAnimationLength = 2.0f; // ���ã����û�ж����¼��������ʱ��

    [Header("��������")]
    public GameObject enemyJoyPrefab;
    public GameObject enemyAngerPrefab;
    public GameObject enemySorrowPrefab;
    public GameObject enemyFearPrefab;

    [Tooltip("��ѡ�����ڷ������ɵ㣬����ʹ����Щ�㡣��Ϊ���������ĸ����������")]
    public Transform[] spawnPoints;

    [Tooltip("ʵ�����ĵ��˽���Ϊ�ø�������Ӷ�����Ϊ�����Զ�����")]
    public Transform enemyParent;

    [Tooltip("���ɵ���������Χ������ min �� max��")]
    public int minEnemies = 4;
    public int maxEnemies = 5;

    [Tooltip("����ս���󶳽�ʱ�����룬��ʵʱ�䣬���� Time.timeScale Ӱ�죩")]
    public float freezeDuration = 2f;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private void Start()
    {
        // �����������ɵ��˲��������룬����ս����������
        StartCoroutine(SpawnAndFreezeCoroutine());
    }

    private IEnumerator SpawnAndFreezeCoroutine()
    {
        // ȷ�� parent ����
        if (enemyParent == null)
        {
            var go = new GameObject("Enemies");
            DontDestroyOnLoad(go); // ��ѡ��������뱣���糡������������ע�͵�
            enemyParent = go.transform;
        }

        SpawnEnemies();

        // ʱ�䶳�ᣨ��ʵʱ��ȴ���
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = 1f;

        // �������������������ʼ������������ AI��HUD ��ʾ�ȣ�
        yield break;
    }

    private void SpawnEnemies()
    {
        // ѡ������������������
        int count = Random.Range(minEnemies, maxEnemies + 1);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos = ChooseSpawnPosition(i, count);
            GameObject prefab = ChooseRandomEnemyPrefab();
            if (prefab == null)
            {
                Debug.LogWarning("BattleSceneController: ĳ������Ԥ����Ϊ�գ����� Inspector �����á�");
                continue;
            }

            GameObject inst = Instantiate(prefab, pos, Quaternion.identity, enemyParent);
            spawnedEnemies.Add(inst);
        }
    }

    private Vector3 ChooseSpawnPosition(int index, int total)
    {
        // ����ʹ�����õ� spawnPoints
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            // ��� spawnPoints ��������������ѭ��ʹ�û������ȡ
            int choose = spawnPoints.Length >= total ? index % spawnPoints.Length : Random.Range(0, spawnPoints.Length);
            return spawnPoints[choose].position;
        }

        // �����ڳ������ĸ����������λ�ã�X ���ɢ��Y �����뱾������ͬ��
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

    // ԭ�нӿڣ��ɾ���ս���߼���ս������ʱ���ã�ʤ����ʧ�ܣ�
    public void OnBattleEnded(bool victory)
    {
        // ���� CheckpointManager ս�������Manager ����ñ���� PlayOutcomeAnimation�����ҵ���
        CheckpointManager.ReportBattleResult(victory);
    }

    // CheckpointManager ���ܻ�ֱ�ӵ��ô˷��������Ŷ�Ӧ�Ķ���
    public void PlayOutcomeAnimation(bool victory)
    {
        if (animator != null)
        {
            animator.SetTrigger(victory ? "Victory" : "Failure");
            // ������ж����¼�����ֱ�ӵ��� CheckpointManager.NotifyBattleAnimationComplete��
            // ������Э�̵ȴ�һ���̶�ʱ����֪ͨ��������ʾ��
            StartCoroutine(WaitAndNotify(victory));
        }
        else
        {
            // ���û�� animator��ֱ��֪ͨ�����⿨ס��
            CheckpointManager.NotifyBattleAnimationComplete(victory);
        }
    }

    private IEnumerator WaitAndNotify(bool victory)
    {
        // ʹ����ʵʱ��ȴ����������������������� Time.timeScale; ������ unscaled��
        yield return new WaitForSecondsRealtime(outcomeAnimationLength);
        CheckpointManager.NotifyBattleAnimationComplete(victory);
    }

    // ��ѡ���ṩ�ⲿ����������������Ҫ��ս�����������ٲ�������ʱ���ã�
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