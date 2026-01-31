using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    [Header("UI References")]
    [Tooltip("用来放大的中间贴图")]
    public RectTransform transitionImageRect; 
    [Tooltip("用来控制整体透明度的CanvasGroup")]
    public CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    [Tooltip("放大覆盖全屏所需时间")]
    public float expandDuration = 0.5f;
    [Tooltip("切换完场景后淡出所需时间")]
    public float fadeOutDuration = 0.5f;
    [Tooltip("最大缩放倍数，确保能覆盖全屏 (通常20-50之间，取决于贴图大小)")]
    public float maxScale = 50f;
    
    [Tooltip("动画曲线，建议设置成 EaseInOut 让效果更丝滑")]
    public AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private void Awake()
    {
        // 单例模式：确保全局只有一个转场管理器
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 切换场景时不销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // 初始状态：隐藏 UI，重置参数
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false; // 避免阻挡点击
        transitionImageRect.localScale = Vector3.zero;
    }

    /// <summary>
    /// 公开方法：调用此方法切换场景
    /// </summary>
    /// <param name="sceneName">目标场景名称</param>
    public void SwitchScene(string sceneName)
    {
        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        // 1. 准备开始：阻挡射线，防止玩家重复点击
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1; // 确保可见
        transitionImageRect.localScale = Vector3.zero; // 从0开始

        // 2. 动画阶段一：贴图放大 (Scale Up)
        float timer = 0f;
        while (timer < expandDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / expandDuration;
            float curveValue = animationCurve.Evaluate(progress);

            // 插值计算缩放
            float currentScale = Mathf.Lerp(0f, maxScale, curveValue);
            transitionImageRect.localScale = new Vector3(currentScale, currentScale, 1f);

            yield return null;
        }

        // 确保放大到最大
        transitionImageRect.localScale = new Vector3(maxScale, maxScale, 1f);

        // 3. 加载场景 (Async Load)
        // 此时屏幕已经被贴图完全遮挡，可以安全加载场景了
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false; // 暂时不激活，等待加载进度

        // 等待加载进度达到 90% (Unity中加载到0.9即代表准备完毕)
        while (op.progress < 0.9f)
        {
            yield return null;
        }

        // 允许激活新场景
        op.allowSceneActivation = true;

        // 等待新场景完全初始化
        while (!op.isDone)
        {
            yield return null;
        }

        // 4. 动画阶段二：淡出 (Fade Out) 
        // 你也可以选择缩小(Scale Down)，这里按你要求做的淡出
        timer = 0f;
        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeOutDuration;
            // 反向插值 1 -> 0
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);
            yield return null;
        }

        // 5. 结束：完全隐藏
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        transitionImageRect.localScale = Vector3.zero;
    }
}