using System.Collections;
using UnityEngine;

public class BattleSceneController : MonoBehaviour
{
    public Animator animator; // 在 Inspector 挂入，用于播放 Victory/Failure 动画
    public float outcomeAnimationLength = 2.0f; // 如果没有用 AnimatorState 长度，可用此硬编码时长

    // 由具体战斗逻辑在战斗结束时调用（胜利或失败）
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
        yield return new WaitForSeconds(outcomeAnimationLength);
        CheckpointManager.NotifyBattleAnimationComplete(victory);
    }
}