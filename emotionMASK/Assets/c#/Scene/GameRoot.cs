using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 管理全局
/// </summary>
public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance {  get; private set; }

    //场景管理器
    public SceneSystem sceneSystem {  get; private set; }


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        sceneSystem = new SceneSystem();

        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        sceneSystem.SetScene(new StartScene());
    }
}
