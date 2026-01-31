using UnityEngine;

public class DialogueController : MonoBehaviour
{
    // 场景开始时执行：读取当前对话索引，并根据索引展示对应内容（UI/文本/音频等）
    public void Start()
    {
        int idx = CheckpointManager.CurrentDialogueIndex;
        Debug.Log("DialogueController: show dialogue index " + idx);
        // TODO: 根据 idx 加载/播放对应的对话文本、语音或动画
    }

    // 对话结束时由 UI 或动画事件调用：通知流程管理器“对话已完成”
    public void OnDialogueFinished()
    {
        CheckpointManager.NotifyDialogueComplete();
    }
}