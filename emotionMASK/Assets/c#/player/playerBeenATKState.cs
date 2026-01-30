using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBeenATKState : playerState
{
    public playerBeenATKState(player player, playerStateMachine stateMachine, string animBoolName) 
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
        
        // // 检测是否离开地面
        // if(!player.IsGroundDetected())
        // {
        //     stateMachine.ChangeState(player.airState);
        //     return;
        // }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
