using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerIdleState : playerState
{
    public playerIdleState(player player, playerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        // player.SetVelocity(0f, 0f);
    }
    public override void Update()
    {
        base.Update();
        
        if(!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
            return;
        }
        
        if(xInput != 0)
            stateMachine.ChangeState(player.moveState);   

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
