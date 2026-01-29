using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 场景状态管理系统
/// </summary>
public class SceneSystem
{
    //场景状态
    SceneState sceneState;


    //生成场景并进入
    public void SetScene(SceneState scene)
    {
        if (sceneState != null)
            sceneState.Exit();

        sceneState = scene;
        
        if(sceneState != null)
            sceneState.Enter();
    }
}
