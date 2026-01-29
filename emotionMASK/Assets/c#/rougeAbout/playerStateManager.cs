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
        Debug.Log("正常状态更新中");
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
        Debug.Log("当前形态：" + PlayerFormManager.playerForm.currentFormIndex);
    }
    
    public static float playerCalculateDamage(float baseDamage)
    {
        bool isCritical = Random.value < critRate;
        if (isCritical)
        {
            return baseDamage * critDamageMultiplier;
        }
        else
        {
            return baseDamage;
        }
    }
    public static void enemyCalculateDamage(float basedamage)
    {
        playerHP -= basedamage;
        if (playerHP < 0)
        {
            playerHP = 0;
        }
        Debug.Log("玩家受到 " + basedamage + " 点伤害，当前血量：" + playerHP);
    }
}
