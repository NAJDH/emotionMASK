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


    public BasePanel(UIType uiType)
    {
        this.uiType = uiType;
    }


    public virtual void Enter()  //UI进入时执行的操作，只执行一次
    {

    }

    public virtual void Pause()  //UI暂停时执行的操作
    {

    }

    public virtual void Resume()  //UI继续时的操作
    {

    }

    public virtual void Exit()  // UI退出时的操作
    {

    }
}
