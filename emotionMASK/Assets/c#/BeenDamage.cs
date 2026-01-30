using UnityEngine;

// 1. 先定义枚举 MaskType
// 把它放在类的外面，这样所有脚本（玩家、敌人、UI）都能直接用 MaskType，不需要加前缀
public enum MaskType 
{ 
    Joy = 0,    // 喜
    Anger = 1,  // 怒
    Sorrow = 2, // 哀
    Fear = 3    // 惧
}

// 2. 再定义接口 IDamageable
// 也要放在外面，不要包在任何 class 里面！
public interface IDamageable
{
    // 现在的 MaskType 已经定义在上面了，所以这里不会报错了
    void TakeDamage(float amount);
}

// 3. (可选) 如果你不需要挂载这个脚本到物体上，连下面这个类都可以删掉
// 只要保留上面两段代码，这个文件就起作用了
/* public class BeenDamage : MonoBehaviour 
{
    // 这里面是空的也没关系
}
*/