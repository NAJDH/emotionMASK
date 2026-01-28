using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 开始主面板
/// </summary>
public class StartPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/StartPanel";

    public StartPanel() : base(new UIType(path))
    {
    }


    public override void Enter()
    {
        base.Enter();

        uiTool.GetOrAddComponentInChildren<Button>("gameStartButton").onClick.AddListener(() =>
        {
            //按钮点击事件
            Debug.Log("The game-start-button was clicked!");
        });
    }

}
