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
        // Debug.Log($"移动状态更新 | xInput={xInput} | moveSpeed={playerStateManager.moveSpeed}");
        
        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
        
        float targetVelocity = xInput * playerStateManager.moveSpeed;
        player.SetVelocity(targetVelocity, player.rb.velocity.y);
        
        // Debug.Log($"设置速度后 | rb.velocity.x = {player.rb.velocity.x} | 目标速度 = {targetVelocity}");
        
        if(xInput == 0)
            stateMachine.ChangeState(player.idleState);  

        if(player.ConsumeBufferedJump() && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
        if (player.ConsumeBufferedAtk1())
        {
            stateMachine.ChangeState(player.normalATKState);
        }
        if(player.ConsumeBufferedAtk2())
        {
            stateMachine.ChangeState(player.normalATK2);
        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}
