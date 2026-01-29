using UnityEngine;

// [CreateAssetMenu] 是核心！
// 这行代码告诉 Unity：“请在右键菜单里加一个选项，让我能创建这种文件。”
// fileName 是默认文件名，menuName 是右键菜单的路径。
[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Rogue/Upgrade")]
public class RogueUpgradeData : ScriptableObject // 注意：这里继承的是 ScriptableObject，而不是 MonoBehaviour
{
    [Header("UI 显示信息")] 
    public string upgradeName;      // 卡片名字，比如 "狂暴之力"
    [TextArea] public string description; // 卡片描述，比如 "攻击力增加 50%，但防御力降低"
    public Sprite icon;             // 卡片上的图标图片

    [Header("逻辑数值")]
    public UpgradeType type;        // 这张卡的功能类型（是加攻？还是回血？）
    public float value;             // 具体加多少？（比如 0.2 代表 20%）
}


public enum UpgradeType
{
    // --- 基础数值类 ---
    AttackUp,
    SpeedUp,
    Heal,
    
    // --- 新增：特殊机制类 ---
    CritRateUp,      // 增加暴击率 (加到100%就是必定暴击)
    DoubleStrike,    // 解锁双重打击 (一次攻击造成两次伤害)
    LifeSteal,        // 吸血机制
    AddATKForm      // 增加一种攻击形态
}