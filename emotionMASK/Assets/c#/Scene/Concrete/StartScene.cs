using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 开始场景
/// </summary>
public class StartScene : SceneState
{
    readonly string sceneName = "StartScene";
    Panel_Manager panelManager;

    public override void Enter()
    {
        panelManager = new Panel_Manager();

        if(SceneManager.GetActiveScene().name != sceneName)
        {
            SceneManager.LoadScene(sceneName);
            SceneManager.sceneLoaded += SceneLoaded;
        }
        else
            panelManager.Push(new StartPanel());
    }

    public override void Exit()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }


    //场景加载后执行
    private void SceneLoaded(Scene scene, LoadSceneMode load)
    {
        Debug.Log($"The scene named {sceneName} was finished loading!");
        panelManager.Push(new StartPanel());
    }
}
