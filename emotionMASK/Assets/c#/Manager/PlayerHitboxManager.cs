using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxManager : MonoBehaviour
{
    // 定义一个简单的类，用来在Inspector里配置名字和对应的脚本
    [System.Serializable]
    public class NamedHitbox
    {
        public string hitboxName;       // 比如 "NormalAttack", "UpAttack"
        public PlayerHitbox hitboxScript; // 拖拽挂载了 PlayerHitbox 的子物体
    }

    [Header("攻击判定框列表")]
    public List<NamedHitbox> hitboxes = new List<NamedHitbox>();

    // 用字典来快速查找（优化性能）
    private Dictionary<string, PlayerHitbox> hitboxDict = new Dictionary<string, PlayerHitbox>();

    private void Awake()
    {
        // 初始化字典，方便后续通过名字查找
        foreach (var h in hitboxes)
        {
            if (!hitboxDict.ContainsKey(h.hitboxName))
            {
                hitboxDict.Add(h.hitboxName, h.hitboxScript);
                // 确保游戏开始时所有框都是关闭的
                h.hitboxScript.gameObject.SetActive(false); 
            }
        }
    }

    /// <summary>
    /// 开启指定名字的攻击框
    /// </summary>
    public void EnableHitbox(string name)
    {
        if (hitboxDict.TryGetValue(name, out PlayerHitbox hitbox))
        {
            hitbox.gameObject.SetActive(true);
            // // 这里很重要：每次开启时，重置它的命中状态，确保能再次造成伤害
            // hitbox.hasHitEnemy = false; 
        }
        else
        {
            Debug.LogWarning($"未找到名为 {name} 的攻击框，请检查 Inspector 配置！");
        }
    }

    /// <summary>
    /// 关闭指定名字的攻击框
    /// </summary>
    public void DisableHitbox(string name)
    {
        if (hitboxDict.TryGetValue(name, out PlayerHitbox hitbox))
        {
            hitbox.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 关闭所有攻击框（通常在退出状态时调用，以防万一）
    /// </summary>
    public void DisableAllHitboxes()
    {
        foreach (var kvp in hitboxDict)
        {
            kvp.Value.gameObject.SetActive(false);
        }
    }
}