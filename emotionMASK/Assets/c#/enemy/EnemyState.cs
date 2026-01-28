using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    // protected EnemyAnimationEvent EnemyAnimationEvent => enemybase.GetComponentInChildren<EnemyAnimationEvent>();
    protected EnemyStateMachine stateMachine;
    protected Enemy enemybase;
    protected string animBoolName;
    protected float stateTimer;

    protected bool movementOver;
    
    public EnemyState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName)
    {
        this.enemybase = enemybase;                           
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    #region 状态机的三个核心函数
    public virtual void Enter()                       //进入状态
    {
        movementOver = false;
        enemybase.anim.SetBool(animBoolName, true);
    }
    public virtual void Update()                      //更新状态
    {
        stateTimer -= Time.deltaTime;

        float battleMoveSpeedMultiplier = enemybase.battleMoveSpeed / enemybase.moveSpeed; 

        enemybase.anim.SetFloat("xVelocity", enemybase.rb.velocity.x);
        enemybase.anim.SetFloat("moveAnimSpeedMultiplier", enemybase.moveAnimSpeedMultiplier);
        enemybase.anim.SetFloat("battleMoveAnimSpeedMultiplier", battleMoveSpeedMultiplier);
    }
    public virtual void Exit()                        //退出状态
    {
        enemybase.anim.SetBool(animBoolName, false);
    }
    #endregion

    public void MovementOver()
    {
        movementOver = true;
    }
}
