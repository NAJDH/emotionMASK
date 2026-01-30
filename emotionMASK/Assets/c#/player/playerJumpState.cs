using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerJumpState : playerState
{
    public playerJumpState(player player, playerStateMachine stateMachine, string animBoolName) 
        : base(player, stateMachine, animBoolName)
    {
        
    }
    public override void Enter()
    {
        base.Enter();
        AudioManager.PlayAudio("jump");
        player.rb.velocity = new Vector2(player.rb.velocity.x, playerStateManager.jumpForce); //设置跳跃速度
    }
    public override void Update()
    {
        base.Update();
        
        // 添加空中水平移动
        if(xInput != 0)
        {
            player.SetVelocity(playerStateManager.moveSpeed * xInput * 0.8f, player.rb.velocity.y);
        }
        
        if(player.rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
