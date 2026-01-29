using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 场景状态
/// </summary>
public abstract class SceneState
{
    //场景进入
    public abstract void Enter();

    //场景退出
    public abstract void Exit();
}
