// FixedFormEnemy.cs - 固定形态敌人接口
using UnityEngine;

public interface IFixedFormEnemy
{
    MaskType FixedForm { get; }
    string EnemyTypeName { get; }
    void OnFormAbilityTrigger();
}