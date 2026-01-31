using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class CheckpointManager
{
    // 入口/流程用到的场景名（请按项目实际场景名修改）
    public static string StartScene = "StartScene";
    public static string FixedIntroScene = "IFeelUncomfortable";
    public static string DialogueScene = "say";
    public static string BattleScene = "SampleScene";

    // 当前对话的索引（0..7），由 ChooseNextDialogue() 随机决定
    public static int CurrentDialogueIndex { get; private set; } = 0;

    // 隐藏的 Runner，用来在静态类里启动协程
    private static CheckpointRunner runner;

    // 关卡流程状态标记（由对应场景的脚本回调通知）
    private static bool fixedIntroCompleted;       // 固定序列/开场是否完成
    private static bool dialogueCompleted;         // 对话是否完成
    private static bool battleAnimationCompleted;  // 战斗结算动画是否完成
    private static bool lastBattleResultVictory;   // 上一次战斗是否胜利

    // 总流程是否在运行（防止重复启动）
    private static bool running;

    // 确保协程 Runner 存在且常驻（DontDestroyOnLoad）
    private static void EnsureRunner()
    {
        if (runner != null) return;
        var go = new GameObject("CheckpointManagerRunner");
        GameObject.DontDestroyOnLoad(go);
        runner = go.AddComponent<CheckpointRunner>();
        runner.hideFlags = HideFlags.HideInHierarchy;
    }

    // 外部入口：从开始按钮/开场触发整个流程
    public static void StartRun()
    {
        if (running) return;
        EnsureRunner();
        running = true;

        // 重置流程状态
        fixedIntroCompleted = false;
        dialogueCompleted = false;
        battleAnimationCompleted = false;

        // 启动流程主协程
        runner.StartCoroutine(RunCoroutine());
    }

    // 随机选择下一段对话索引（0..7）
    private static void ChooseNextDialogue()
    {
        CurrentDialogueIndex = Random.Range(0, 8); // 0..7 共 8 段对话
    }

    // 固定开场完成通知（由场景脚本调用）
    public static void NotifyFixedIntroComplete()
    {
        fixedIntroCompleted = true;
    }

    // 对话完成通知（由对话 UI/脚本调用）
    public static void NotifyDialogueComplete()
    {
        dialogueCompleted = true;
    }

    // 战斗结算动画完成通知（胜/负由参数传入）
    public static void NotifyBattleAnimationComplete(bool victory)
    {
        lastBattleResultVictory = victory;
        battleAnimationCompleted = true;
    }

    // 战斗结果上报：由战斗场景触发
    public static void ReportBattleResult(bool victory)
    {
        lastBattleResultVictory = victory;

        // 优先让 BattleSceneController 播放胜/负结算动画
        var controller = Object.FindObjectOfType<BattleSceneController>();
        if (controller != null)
        {
            controller.PlayOutcomeAnimation(victory);
            return;
        }

        // 如果找不到控制器，直接标记完成，避免流程卡住
        battleAnimationCompleted = true;
    }

    // 主流程协程：开场 -> 对话 -> 战斗 -> 结算 -> 继续/结束
    private static IEnumerator RunCoroutine()
    {
        // 1) 加载固定开场场景
        yield return runner.StartCoroutine(LoadSceneCO(FixedIntroScene));

        // 等待固定开场完成（带超时，避免卡死）
        fixedIntroCompleted = false;
        float fixedTimeout = 10f;
        float t0 = Time.time;
        while (!fixedIntroCompleted && Time.time - t0 < fixedTimeout)
            yield return null;

        // 2) 循环：对话 -> 战斗
        while (running)
        {
            // 选择下一段对话
            ChooseNextDialogue();

            // 加载对话场景
            yield return runner.StartCoroutine(LoadSceneCO(DialogueScene));

            // 等待对话完成（带超时）
            dialogueCompleted = false;
            float dialogueTimeout = 60f;
            t0 = Time.time;
            while (!dialogueCompleted && Time.time - t0 < dialogueTimeout)
                yield return null;

            // 加载战斗场景
            yield return runner.StartCoroutine(LoadSceneCO(BattleScene));

            // 等待战斗结算动画完成（带超时）
            battleAnimationCompleted = false;
            float battleTimeout = 120f;
            t0 = Time.time;
            while (!battleAnimationCompleted && Time.time - t0 < battleTimeout)
                yield return null;

            // 根据胜负决定是否继续
            if (lastBattleResultVictory)
            {
                // 胜利：回到下一轮对话
                continue;
            }
            else
            {
                // 失败：回到开始场景并结束流程
                yield return runner.StartCoroutine(LoadSceneCO(StartScene));
                running = false;
                yield break;
            }
        }
    }

    // 异步加载场景（等待加载完成）
    private static IEnumerator LoadSceneCO(string sceneName)
    {
        var op = SceneManager.LoadSceneAsync(sceneName);
        if (op == null)
            yield break;
        while (!op.isDone)
            yield return null;
    }

    // 内部 MonoBehaviour：仅用于启动协程
    private class CheckpointRunner : MonoBehaviour { }
}