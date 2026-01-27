using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    // 单例实例，方便全局访问
    public static HitStopManager Instance { get; private set; }

    [Header("组件设置")]
    [Tooltip("用于播放攻击音效的音源")]
    public AudioSource sfxAudioSource;

    // 记录是否正在顿帧中，防止逻辑冲突
    private bool isWaiting = false;

    private void Awake()
    {
        // 初始化单例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayHitStop()
    {
        TriggerHitStop(0.2f, sfxAudioSource.clip);
    }

    /// <summary>
    /// 触发顿帧和音效
    /// </summary>
    /// <param name="duration">顿帧持续时间（秒），例如 0.2</param>
    /// <param name="attackClip">要播放的攻击音效</param>
    public void TriggerHitStop(float duration, AudioClip attackClip)
    {
        // 1. 播放音效 (不受 TimeScale 影响)
        if (attackClip != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(attackClip);
        }

        // 2. 如果已经在顿帧，先停止之前的协程，重置时间，确保新的顿帧生效
        if (isWaiting)
        {
            StopAllCoroutines();
            Time.timeScale = 1f; 
        }

        // 3. 开启协程处理时间暂停
        StartCoroutine(DoHitStop(duration));
    }

    // 执行顿帧逻辑的协程
    private IEnumerator DoHitStop(float duration)
    {
        isWaiting = true;

        // 瞬间暂停游戏
        Time.timeScale = 0f;

        // 使用 WaitForSecondsRealtime，因为 WaitForSeconds 会受 timeScale=0 影响而永远暂停
        yield return new WaitForSecondsRealtime(duration);

        // 恢复游戏时间
        Time.timeScale = 1f;
        
        isWaiting = false;
    }
}