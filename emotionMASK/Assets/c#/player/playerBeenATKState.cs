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
        player.SetVelocity(0f, 0f);
    }
    public override void Update()
    {
        base.Update();
        // 不要在这里重置 isBeHit，否则受击状态立即退出
        if(playerStateManager.isBeHit == false)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }
}
