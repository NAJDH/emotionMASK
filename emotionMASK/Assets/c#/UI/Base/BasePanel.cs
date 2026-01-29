using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 所有UI面板的父类，包含UI面板的状态信息
/// </summary>
public class BasePanel
{
    //UI信息
    public UIType uiType {  get; private set; }
    //UI管理工具
    public UI_Tool uiTool { get; private set; }
    //面板管理器
    public Panel_Manager panelManager { get; private set; }
    //UI管理器
    public UI_Manager uiManager { get; private set; }


    //初始化uiTool
    public void Initialize(UI_Tool tool)
    {
        uiTool = tool;
    }


    //初始化画面管理器
    public void Initialize(Panel_Manager panelManager)
    {
        this.panelManager = panelManager;
    }


    //初始化UI管理器
    public void Initialize(UI_Manager uiManager)
    {
        this.uiManager = uiManager;
    }


    public BasePanel(UIType uiType)
    {
        this.uiType = uiType;
    }


    public virtual void Enter()  //UI进入时执行的操作，只执行一次
    {

    }

    public virtual void Pause()  //UI暂停时执行的操作
    {
        uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public virtual void Resume()  //UI继续时的操作
    {
        uiTool.GetOrAddComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public virtual void Exit()  // UI退出时的操作
    {
        uiManager.DestroyUI(uiType);
    }
}
