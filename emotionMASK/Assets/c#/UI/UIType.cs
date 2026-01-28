using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存储单个UI的信息，包括名字和路径
/// </summary>
public class UIType
{
    //UI名字
    public string name { get;private set; }

    //UI路径
    public string path { get; private set; }

    public UIType(string path)
    {
        this.path = path;
        this.name = path.Substring(path.LastIndexOf('/') + 1);
    }
}
