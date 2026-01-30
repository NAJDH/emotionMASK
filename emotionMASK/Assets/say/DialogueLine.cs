using UnityEngine;

/* [System.Serializable] // 必须加上这个，才能在 Inspector 面板中看到和编辑
public class DialogueLine
{
    [Tooltip("角色名字")]
    public string characterName;

    [Tooltip("对话内容")]
    [TextArea(3, 10)] // 让输入框大一点，方便编辑多行文本
    public string sentence;

    [Tooltip("角色立绘 (可选)")]
    public Sprite characterSprite; 
} */



using System.Collections.Generic;

// 1. 定义位置枚举
public enum CharacterPosition
{
    Left,  // 左边
    Right  // 右边
}

// 2. 升级单句对话的数据结构
[System.Serializable]
public class DialogueLine
{
    [Header("角色信息")]
    public string characterName;      // 名字
    public CharacterPosition position; // 这句话是谁说的（在左边还是右边？）
    public Sprite characterSprite;    // 对应的立绘（如果为空，则保持上一张不变）

    [Header("对话内容")]
    [TextArea(3, 10)]
    public string sentence;
}

// 3. 新增：一个“剧本”容器
// 用这个类来代表“一段完整的独立剧情”
[System.Serializable]
public class StoryChapter
{
    public string chapterName; // 给剧情起个名字（方便你在编辑器里看，比如"初次见面", "吵架剧情"）
    public DialogueLine[] lines; // 这段剧情里的所有对话
}