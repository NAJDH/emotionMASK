using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTransformState : playerState
{
    private GameObject targetForm; // 目标形态对象
    
    public playerTransformState(player _player, playerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 动画播放完成后会触发AnimEvent
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        
        // 动画播放完成后自动退出（通过AnimEvent触发）
    }
    
    public void SetTargetForm(GameObject form)
    {
        targetForm = form;
    }
    
    // 在动画结束时调用（通过AnimEvent）
    public void CompleteTransform()
    {
        if (targetForm != null)
        {
            // 将目标形态移动到当前玩家位置
            targetForm.transform.position = player.transform.position;
            targetForm.SetActive(true);
            
            // 将当前玩家移到远处
            player.transform.position = new Vector3(10000f, 10000f, 0f);
            
            // 切换控制权
            PlayerFormManager.Instance.SwitchControl(targetForm);
        }
    }
}