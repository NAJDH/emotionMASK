using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void ShowMainMenu() { Debug.Log("UI: 显示主菜单"); }
    public void ShowIntro() { Debug.Log("UI: 显示前情提要"); }
    public void ShowDialogue() { Debug.Log("UI: 显示剧情"); }
    public void HideAllUI() { Debug.Log("UI: 隐藏所有UI进入战斗"); }
    
    public void ShowSettlement(bool isWin) 
    { 
        Debug.Log($"UI: 显示结算，玩家赢了吗？{isWin}"); 
    }
}