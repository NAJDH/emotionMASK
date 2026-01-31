using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CheckpointManager
{
    // 场景名 — 根据你的项目实际场景名修改
    public static string StartScene = "StartScene";
    public static string FixedIntroScene = "IFeelUncomfortable";
    public static string DialogueScene = "say";
    public static string BattleScene = "SampleScene";

    // 当前随机对话索引（0..7）
    public static int CurrentDialogueIndex { get; private set; } = 0;

    // internal runner
    private static CheckpointRunner runner;

    // 流程状态标志（由场景内脚本回调设置）
    private static bool fixedIntroCompleted;
    private static bool dialogueCompleted;
    private static bool battleAnimationCompleted;
    private static bool lastBattleResultVictory;

    private static bool running;

    // 初始化 runner（单例、常驻）
    private static void EnsureRunner()
    {
        if (runner != null) return;
        var go = new GameObject("CheckpointManagerRunner");
        GameObject.DontDestroyOnLoad(go);
        runner = go.AddComponent<CheckpointRunner>();
        runner.hideFlags = HideFlags.HideInHierarchy;
    }

    // 外部入口：从开始界面点击开始时调用
    public static void StartRun()
    {
        if (running) return;
        EnsureRunner();
        running = true;
        fixedIntroCompleted = false;
        dialogueCompleted = false;
        battleAnimationCompleted = false;
        runner.StartCoroutine(RunCoroutine());
    }

    // 随机选择下一个对话
    private static void ChooseNextDialogue()
    {
        CurrentDialogueIndex = Random.Range(0, 8); // 0..7 共8个对话
    }

    // 场景回调：固定过场动画场景在动画完成时调用
    public static void NotifyFixedIntroComplete()
    {
        fixedIntroCompleted = true;
    }

    // 场景回调：对话场景完成（例如玩家点击继续）时调用
    public static void NotifyDialogueComplete()
    {
        dialogueCompleted = true;
    }

    // 场景回调：战斗动画播放结束时调用（胜利或失败动画都调用，并传入结果）
    public static void NotifyBattleAnimationComplete(bool victory)
    {
        lastBattleResultVictory = victory;
        battleAnimationCompleted = true;
    }

    // 战斗场景在战斗结束后调用本方法，CheckpointManager 会调用场景内的 BattleSceneController 播放结果动画
    public static void ReportBattleResult(bool victory)
    {
        lastBattleResultVictory = victory;
        // 找到场景内的 BattleSceneController（若存在）并让它播放对应动画
        var controller = Object.FindObjectOfType<BattleSceneController>();
        if (controller != null)
        {
            controller.PlayOutcomeAnimation(victory);
            return;
        }

        // 如果没有 controller 直接通知（保证流程不会卡住）
        battleAnimationCompleted = true;
    }

    // 主流程协程（由 runner 启动）
    private static IEnumerator RunCoroutine()
    {
        // 1) 加载固定过场场景
        yield return runner.StartCoroutine(LoadSceneCO(FixedIntroScene));

        // 等待过场场景通过 NotifyFixedIntroComplete 通知完成（可考虑超时）
        fixedIntroCompleted = false;
        float fixedTimeout = 10f;
        float t0 = Time.time;
        while (!fixedIntroCompleted && Time.time - t0 < fixedTimeout)
            yield return null;

        // 2) 首次进入对话（或循环中每次都重新选择）
        while (running)
        {
            ChooseNextDialogue();

            // 加载对话场景
            yield return runner.StartCoroutine(LoadSceneCO(DialogueScene));

            // 等待对话场景通知完成
            dialogueCompleted = false;
            float dialogueTimeout = 60f;
            t0 = Time.time;
            while (!dialogueCompleted && Time.time - t0 < dialogueTimeout)
                yield return null;

            // 加载战斗场景
            yield return runner.StartCoroutine(LoadSceneCO(BattleScene));

            // 在战斗场景中，BattleSceneController 会在战斗结束时调用 ReportBattleResult
            // 然后由 BattleSceneController 播放胜/败动画，并在动画结束时调用 NotifyBattleAnimationComplete
            battleAnimationCompleted = false;
            float battleTimeout = 120f;
            t0 = Time.time;
            while (!battleAnimationCompleted && Time.time - t0 < battleTimeout)
                yield return null;

            // 根据上一次结果跳转：
            if (lastBattleResultVictory)
            {
                // 胜利：重新分配随机对话并继续循环（加载对话场景）
                // loop will choose next dialogue automatically
                continue;
            }
            else
            {
                // 失败：返回开始界面并结束流程
                yield return runner.StartCoroutine(LoadSceneCO(StartScene));
                running = false;
                yield break;
            }
        }
    }

    // 场景加载辅助
    private static IEnumerator LoadSceneCO(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        if (op == null)
            yield break;
        while (!op.isDone)
            yield return null;
    }

    // 内部 MonoBehaviour 用于启动协程
    private class CheckpointRunner : MonoBehaviour { }
}