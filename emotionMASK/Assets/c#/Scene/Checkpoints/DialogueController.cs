using UnityEngine;

public class DialogueController : MonoBehaviour
{
    // 用于演示：你可以在 Inspector 中设置 UI 元素并根据 index 展示内容
    public void Start()
    {
        int idx = CheckpointManager.CurrentDialogueIndex;
        Debug.Log("DialogueController: show dialogue index " + idx);
        // TODO: 根据 idx 加载并播放对应对话文本/音频
    }

    // 在对话结束（例如玩家点击继续或对话播放完）时调用
    public void OnDialogueFinished()
    {
        CheckpointManager.NotifyDialogueComplete();
    }
}