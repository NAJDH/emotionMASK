using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI管理工具，包括获取某个子对象组件的操作
/// </summary>
public class UI_Tool
{
    GameObject activePanel;

    public UI_Tool(GameObject panel)
    {
        activePanel = panel;
    }


    //给当前活动面板获取或添加一个组件
    public T GetOrAddComponent<T>() where T : Component
    {
        if(activePanel.GetComponent<T>() == null)
            activePanel.AddComponent<T>();

        return activePanel.GetComponent<T>();
    }


    //根据名称查找子对象
    public GameObject FindChildGameObject(string name)
    {
        Transform[] trans = activePanel.GetComponentsInChildren<Transform>();

        foreach(Transform item in trans)
        {
            if (item.name == name)
                return item.gameObject;
        }

        Debug.LogWarning($"Can't find the ChildGameObject named {name}!!!");
        return null;
    }


    //根据名称获取或添加子对象组件
    public T GetOrAddComponentInChildren<T>(string name) where T : Component
    {
        GameObject child = FindChildGameObject(name);

        if(child != null)
        {
            if(child.GetComponent<T>() == null)
                child.AddComponent<T>();

            return child.GetComponent<T>();
        }

        return null;
    }
}
