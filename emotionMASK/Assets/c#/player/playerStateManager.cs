using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class playerStateManager
{

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
    }
    
}
