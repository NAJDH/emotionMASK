using UnityEngine;

public class StartPanelExtensions : MonoBehaviour
{
    // 把这个方法绑定到开始按钮的 OnClick
    public void OnStartButtonClicked()
    {
        CheckpointManager.StartRun();
    }
}