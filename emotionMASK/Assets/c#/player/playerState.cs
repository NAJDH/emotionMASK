using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerState
{
    // protected AnimEventFunction animEventFunction => player.GetComponentInChildren<AnimEventFunction>();
    protected float xInput;
    protected float yInput;
    public float stateTimer;                    //状态时间，可能会用得上
    #region 状态机所需的变量
    protected playerStateMachine stateMachine;
    protected player player;
    private string animBoolName;
    #endregion

    #region 状态机构造函数
    //构造函数
    public playerState(player player, playerStateMachine stateMachine, string animBoolName)
    {
        this.player = player;                           
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    #endregion
    #region 状态机的三个核心函数
    public virtual void Enter()                       //进入状态
    {
        player.anim.SetBool(animBoolName, true);
    }
    public virtual void Update()                      //更新状态
    {
        //在这里获取水平输入，可以在每一个子类状态都被读到，使用
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        stateTimer -= Time.deltaTime;
        player.anim.SetFloat("yVelocity", player.rb.velocity.y); //设置y轴速度参数,用在判断跳跃
    }
    public virtual void Exit()                        //退出状态
    {
        // animEventFunction.AnimationTriggered = false;
        player.anim.SetBool(animBoolName, false);
    }
    #endregion

    public virtual void OnAttackHit(IDamageable target, Collider2D hitInfo)
{
    // 默认什么都不做，留给子类去重写
}
}
