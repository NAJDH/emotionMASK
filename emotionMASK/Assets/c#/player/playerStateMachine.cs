using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStateMachine
{
    public playerState currentState{get; private set;}
    public void Initialize(playerState startingState)    //初始化状态机
    {
        currentState = startingState;
        currentState.Enter();
    }
    public void ChangeState(playerState newState)       //切换状态
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
