using UnityEngine;
using System.Collections;
using Cinemachine;

public class HitStopManager : MonoBehaviour
{
    // 单例实例，方便全局访问
    public static HitStopManager Instance { get; private set; }

    [Header("组件设置")]
    [Tooltip("用于播放攻击音效的音源")]
    public AudioSource sfxAudioSource;

    [Header("摄像机设置")]
    [Tooltip("将你场景中的 Cinemachine Virtual Camera 拖进去")]
    public CinemachineVirtualCamera virtualCamera;
    
    // 缓存震动组件
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

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
    private void Start()
    {
        // 获取摄像机上的噪音组件
        if (virtualCamera != null)
        {
            virtualCameraNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
    }
    public void PlayHitStop()
    {
        TriggerHitStop(0.2f, 3.0f, 0.25f, sfxAudioSource.clip);
    }

    /// <summary>
    /// 触发顿帧和音效
    /// </summary>
    /// <param name="duration">顿帧持续时间（秒），例如 0.2</param>
    /// /// <param name="shakeIntensity">震动强度 (例如 1.0 到 5.0)</param>
    /// <param name="shakeTime">震动持续时间 (通常比顿帧稍长一点，例如 0.25)</param>
    /// <param name="attackClip">要播放的攻击音效</param>
    public void TriggerHitStop(float duration, float shakeIntensity, float shakeTime, AudioClip attackClip)
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

        // 4. 开启震动协程
        StartCoroutine(DoCameraShake(shakeIntensity, shakeTime));
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
    
    // 震动逻辑
    private IEnumerator DoCameraShake(float intensity, float time)
    {
        if (virtualCameraNoise != null)
        {
            // 设置震动强度
            virtualCameraNoise.m_AmplitudeGain = intensity;

            // 等待震动时间（使用 Realtime，即使游戏暂停也能倒计时）
            yield return new WaitForSecondsRealtime(time);

            // 归零，停止震动
            virtualCameraNoise.m_AmplitudeGain = 0f;
        }
    }
}