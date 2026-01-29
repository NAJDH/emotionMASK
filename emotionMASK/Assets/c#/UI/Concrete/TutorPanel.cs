using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorPanel : BasePanel
{
    static readonly string path = "Prefabs/UI/Panel/TutorPanel";

    public TutorPanel() : base(new UIType(path))
    {
    }


    public override void Enter()
    {
        base.Enter();

        uiTool.GetOrAddComponentInChildren<Button>("exitButton").onClick.AddListener(() =>
        {
            //按钮点击事件
            Debug.Log("The exit-button was clicked!");
            panelManager.Pop();
        });
    }
}
