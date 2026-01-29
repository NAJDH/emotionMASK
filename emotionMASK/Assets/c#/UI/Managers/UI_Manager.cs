using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 存储所有UI信息，并可创建和销毁
/// </summary>
public class UI_Manager
{
    //字典存储所有UI信息，每一个UI信息对应一个GameObject
    private Dictionary<UIType, GameObject> uiDic;

    public UI_Manager()
    {
        uiDic = new Dictionary<UIType, GameObject>();
    }


    //获取一个UI对象
    public GameObject GetSingleType(UIType type)
    {
        GameObject parent = GameObject.Find("Canvas(Panel)");
        
        if (!parent)
        {
            Debug.LogError("Can't find the Canvas");
            return null;
        }

        if(uiDic.ContainsKey(type))
            return uiDic[type];

        GameObject ui = GameObject.Instantiate(Resources.Load<GameObject>(type.path), parent.transform);
        ui.name = type.name;
        uiDic.Add(type, ui);
        return ui;
    }


    //销毁UI，根据UI信息去销毁
    public void DestroyUI(UIType type)
    {
        if(uiDic.ContainsKey(type))
        {
            GameObject.Destroy(uiDic[type]);
            uiDic.Remove(type);
        }
    }
}
