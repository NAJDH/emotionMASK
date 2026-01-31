using UnityEngine;

/// <summary>
/// 游戏核心状态机，管理流程、时间暂停和全局音乐切换
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 定义5种游戏状态
    public enum GameState
    {
        MainMenu,       // 开始游戏界面
        Intro,          // 前情提要
        Dialogue,       // 剧情对话
        Gameplay,       // 实体关卡
        Settlement      // 结算画面
    }

    [Header("状态监控")]
    public GameState currentState;

    [Header("设置：音频名称 (需与AudioManager中一致)")]
    public string uiBgmName = "BGM_UI";         // UI状态下的背景音乐名
    public string battleBgmName = "BGM_Battle"; // 战斗状态下的背景音乐名

    [Header("设置：引用其他管理器")]
    public UIManager uiManager;       // 需要你挂载 UI 管理器
    public LevelManager levelManager; // 需要你挂载 关卡/怪物 管理器

    // 内部变量
    private bool hasPlayedIntro = false; // 记录是否播放过前情提要
    private string currentPlayingBgm = ""; // 记录当前正在播放的BGM名字

    private void Awake()
    {
        // 单例模式初始化
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 游戏启动，默认进入主菜单
        // 注意：由于AudioManager是在Start中初始化的，我们稍微延迟一点调用，或者确保GameManager的执行顺序在AudioManager之后
        // 这里直接调用，假设AudioManager初始化够快
        ChangeState(GameState.MainMenu);
    }

    /// <summary>
    /// 切换游戏状态的核心方法
    /// </summary>
    public void ChangeState(GameState newState)
    {
        currentState = newState;

        // --- 1. 处理时间流逝 (UI暂停，游戏进行) ---
        if (currentState == GameState.Gameplay)
        {
            Time.timeScale = 1f; // 恢复时间
        }
        else
        {
            Time.timeScale = 0f; // 暂停时间
        }

        // --- 2. 处理背景音乐切换 ---
        HandleMusicSwitch(newState);

        // --- 3. 处理各状态的具体逻辑 ---
        switch (currentState)
        {
            case GameState.MainMenu:
                Debug.Log("进入：主菜单");
                uiManager?.ShowMainMenu(); 
                // 可以在这里清理上一局的怪物
                break;

            case GameState.Intro:
                Debug.Log("进入：前情提要");
                uiManager?.ShowIntro();
                break;

            case GameState.Dialogue:
                Debug.Log("进入：剧情对话");
                uiManager?.ShowDialogue();
                // 此时通知 LevelManager 准备下一关的数据（但不生成怪物）
                levelManager?.PrepareNextLevel();
                break;

            case GameState.Gameplay:
                Debug.Log("进入：实体关卡");
                uiManager?.HideAllUI(); // 关闭所有遮挡的UI
                // 通知怪物“瞬移”进场
                levelManager?.StartLevel();
                break;

            case GameState.Settlement:
                Debug.Log("进入：结算界面");
                // 具体的输赢逻辑在调用 ChangeState 前已经决定，这里只负责显示
                break;
        }
    }

    /// <summary>
    /// 根据状态智能切换音乐
    /// </summary>
    private void HandleMusicSwitch(GameState state)
    {
        string targetBgm = "";

        // 判断目标状态应该放什么歌
        if (state == GameState.Gameplay)
        {
            targetBgm = battleBgmName;
        }
        else
        {
            // 其他所有状态（主菜单、剧情、结算）都属于 UI BGM
            targetBgm = uiBgmName;
        }

        // 如果目标音乐和当前正在放的不一样，才切换
        if (currentPlayingBgm != targetBgm)
        {
            // 停止旧的
            if (!string.IsNullOrEmpty(currentPlayingBgm))
            {
                AudioManager.StopAudio(currentPlayingBgm);
            }

            // 播放新的
            AudioManager.PlayAudio(targetBgm, false);
            
            // 更新记录
            currentPlayingBgm = targetBgm;
        }
    }

    // --- 以下是供 UI 按钮绑定的公共方法 ---

    // 绑定在【开始游戏】按钮上
    public void OnStartGameButton()
    {
        if (!hasPlayedIntro)
        {
            hasPlayedIntro = true;
            ChangeState(GameState.Intro);
        }
        else
        {
            ChangeState(GameState.Dialogue);
        }
    }

    // 绑定在【前情提要播放结束】事件上
    public void OnIntroFinished()
    {
        ChangeState(GameState.Dialogue);
    }

    // 绑定在【剧情对话结束/开始战斗】按钮上
    public void OnDialogueFinished()
    {
        ChangeState(GameState.Gameplay);
    }

    // 绑定在【结算界面-继续/重试】按钮上 (胜利逻辑)
    public void OnSettlementContinue()
    {
        ChangeState(GameState.Dialogue); // 回到剧情，准备下一关
    }

    // 绑定在【结算界面-返回主菜单】按钮上 (失败逻辑)
    public void OnSettlementBackToMenu()
    {
        ChangeState(GameState.MainMenu);
    }
    
    // 供外部调用：游戏胜利
    public void LevelWin()
    {
        uiManager?.ShowSettlement(true);
        ChangeState(GameState.Settlement);
    }

    // 供外部调用：游戏失败
    public void LevelLose()
    {
        uiManager?.ShowSettlement(false);
        ChangeState(GameState.Settlement);
    }
}