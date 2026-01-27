using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAirState : playerState
{
    public playerAirState(player player, playerStateMachine stateMachine, string animBoolName) 
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
        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
        if(xInput != 0)
        {
            player.SetVelocity(playerStateManager.moveSpeed * xInput * 0.8f, player.rb.velocity.y);
        }
        // if (player.IsWallDetected())
        // {
        //     stateMachine.ChangeState(player.wallSlideState);
        // }
    }
    public override void Exit()
    {
        base.Exit();
    }
}