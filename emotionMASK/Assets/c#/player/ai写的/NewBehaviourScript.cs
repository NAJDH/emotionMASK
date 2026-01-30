// using UnityEngine;

// // 这是一个纯逻辑类，不需要挂在物体上
// public abstract class PlayerBaseForm
// {
//     protected player _player;
//     protected playerStateMachine _stateMachine;

//     // --- 形态独有的数值 ---
//     public abstract float moveSpeed { get; }
//     public abstract float jumpForce { get; }
//     public abstract string modelName { get; } // 用于查找或激活对应的子物体模型

//     // --- 形态持有的状态 ---
//     // 注意：我们将状态的持有权从 Player 移到了 Form 里
//     public playerState idleState { get; protected set; }
//     public playerState moveState { get; protected set; }
//     public playerState jumpState { get; protected set; }
//     public playerState airState { get; protected set; }
//     public playerState attackState { get; protected set; } // 这里的攻击状态会因形态而异

//     public PlayerBaseForm(player player, playerStateMachine stateMachine)
//     {
//         _player = player;
//         _stateMachine = stateMachine;
//     }

//     // 初始化该形态下的所有状态
//     public virtual void InitializeStates()
//     {
//         // 基础移动状态通常是通用的，也可以被重写
//         idleState = new playerIdleState(_player, _stateMachine, "idle");
//         moveState = new playerMoveState(_player, _stateMachine, "move");
//         jumpState = new playerJumpState(_player, _stateMachine, "jump");
//         airState = new playerAirState(_player, _stateMachine, "jump");
        
//         // 攻击状态留给子类去具体实现
//     }

//     // 进入该形态时调用（切换模型、特效）
//     public virtual void EnterForm()
//     {
//         // 1. 隐藏所有模型，显示当前模型 (需要在 Player 里写个方法配合)
//         // _player.SwitchModel(modelName);
//     }
// }