using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TransformState : EnemyState
{
    private MaskType targetForm;

    public Enemy_TransformState(Enemy enemybase, EnemyStateMachine stateMachine, string animBoolName)
        : base(enemybase, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemybase.SetZeroVelocity();

        // 开始变换动画
        enemybase.anim.SetTrigger("Transform");

        // 设置动画参数
        enemybase.anim.SetInteger("TargetForm", (int)targetForm);
    }

    public override void Update()
    {
        base.Update();

        // 变换过程中不可移动
        enemybase.SetZeroVelocity();

        // 变换完成后会通过动画事件调用 CompleteTransform
    }

    public override void Exit()
    {
        base.Exit();
    }

    public void SetTargetForm(MaskType form)
    {
        targetForm = form;
    }

    // 在动画结束时调用（通过AnimEvent）
    public void CompleteTransform()
    {
        //enemybase.CompleteTransform(targetForm);
    }
}