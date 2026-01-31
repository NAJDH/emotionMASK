using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playerStateManager
{

    //这是玩家数值中心
    public static bool XI;
    public static bool NU;
    public static bool AI;
    public static bool JU;
    public static bool isDead = false;
    public static bool isBeHit = false;                 //这个用来标识玩家是否受击,会在执行受击动画的刚开始被重置
    public static bool isBeingHitting = false;          //这个用来标识玩家是否正在受击，会在受击动画的最后用动画事件重置
    [Header("玩家基础属性")]
    public static float playerHP = 100f;               //这个是厌恶值，我写成了血量而已
    public static float maxPlayerHP = 100f;
    public static float baseDamage = 10f;              //基础伤害
    [Header("移动参数")]
    public static float moveSpeed = 10f;
    public static float jumpForce = 16f;
    internal static object currentForm;
    [Header("特殊机制属性")]
    // 暴击率 (0 = 0%, 1 = 100%)
    public static float critRate = 0.2f; 
    // 暴击伤害倍率 (默认 1.5倍)
    public static float critDamageMultiplier = 1.5f; 
    // // 是否开启双重打击
    // public bool hasDoubleStrike = false;
    // // 吸血比例 (0 表示没吸血)
    // public float lifeStealRate = 0f;
    [Header("玩家额外攻击解锁")]
    public static bool canNUcombo = false;

    public static void Update()
    {
        // 🟢 空值检查
        if(PlayerFormManager.playerForm == null) return;
        
        if(PlayerFormManager.playerForm.currentFormIndex == 1)
        {
            XI = true;
        }
        if(PlayerFormManager.playerForm.currentFormIndex == 2)
        {
            NU = true;
        }
        if(PlayerFormManager.playerForm.currentFormIndex == 3)
        {
            AI = true;
        }
        if(PlayerFormManager.playerForm.currentFormIndex == 4)
        {
            JU = true;
        }
    }
    
    public static float playerCalculateDamage(float baseDamage)
    {
        bool isCritical = Random.value < critRate;
        if (isCritical)
        {
            HitStopManager.Instance.TriggerHitStop(2.0f, 0.15f, "baoji", false);
            return baseDamage * critDamageMultiplier;
        }
        else
        {
            return baseDamage;
        }
    }
    public static void enemyHitPlayerDamage(float basedamage)
    {

        if (!isBeingHitting)
        {
            isBeingHitting = true;
            isBeHit = true;
        playerHP -= basedamage;
        if (playerHP < 0)
        {
            playerHP = 0;
            isDead = true;
        }
        // Debug.Log("玩家受到 " + basedamage + " 点伤害，当前血量：" + playerHP);
        }
    }
}
