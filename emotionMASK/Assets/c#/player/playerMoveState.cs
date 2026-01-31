using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMoveState : playerState
{
    public playerMoveState(player player, playerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Update()
    {
        base.Update();
        Debug.Log("移动状态更新");
        
        // 检测是否离开地面
        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
        
        player.SetVelocity(xInput * playerStateManager.moveSpeed, player.rb.velocity.y);
        
        if(xInput == 0)
            stateMachine.ChangeState(player.idleState);  

        if(Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.normalATKState);
        }
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.normalATK2);
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}
