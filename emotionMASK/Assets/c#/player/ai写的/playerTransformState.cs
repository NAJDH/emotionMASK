using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTransformState : playerState
{
    private GameObject targetForm; // 目标形态对象
    private int targetFormIndex; // 目标形态索引
    
    public playerTransformState(player _player, playerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0,0); // 进入形态切换状态时停止移动
        // 播放形态切换动画
        Debug.Log($"开始形态切换动画");
    }

    public override void Exit()
    {
        Debug.Log($"结束形态切换动画");
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        // 等待动画播放完成（通过AnimEvent触发CompleteTransform）
    }
    
    public void SetTargetForm(GameObject form, int formIndex)
    {
        targetForm = form;
        targetFormIndex = formIndex;
    }
    
    // 在动画结束时调用（通过AnimEvent触发）
    public void CompleteTransform()
    {
        if (targetForm != null)
        {
            // 激活目标形态
            targetForm.SetActive(true);
            
            // 通过Manager切换控制权和位置
            PlayerFormManager.playerForm.SwitchControl(targetForm, targetFormIndex);
            
            // 返回idle状态
            player newPlayer = targetForm.GetComponent<player>();
            if (newPlayer != null)
            {
                newPlayer.stateMachine.ChangeState(newPlayer.idleState);
            }
        }
    }
}