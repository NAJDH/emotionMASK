using UnityEngine;

// 敌人的形态基类
public abstract class EnemyBaseForm
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;

    // 形态属性
    public abstract MaskType FormType { get; }
    public abstract Color FormColor { get; }  //变色
    public abstract float MoveSpeedMultiplier { get; }  //移速
    public abstract float DamageMultiplier { get; }  //伤害倍率
    public abstract float HealthMultiplier { get; }  //生命值倍率

    //// 形态专属状态
    //public EnemyState idleState { get; protected set; }
    //public EnemyState moveState { get; protected set; }
    //public EnemyState attackState { get; protected set; }
    //public EnemyState transformState { get; protected set; }

    public EnemyBaseForm(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }

    // 初始化该形态的状态
    public virtual void InitializeStates()
    {
        // 基础状态
        //idleState = new Enemy_IdleState(enemy, stateMachine, "idle");
        //moveState = new Enemy_MoveState(enemy, stateMachine, "move");

        // 攻击状态由子类实现
    }

    // 进入形态
    public virtual void EnterForm()
    {
        // 更新颜色
        SpriteRenderer sr = enemy.GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = FormColor;

        // 更新动画参数
        enemy.anim.SetInteger("FormType", (int)FormType);
    }

    // 形态特殊技能
    //public abstract void UseSpecialAbility();
}

// 具体的敌人形态实现
public class EnemyJoyForm : EnemyBaseForm
{
    public override MaskType FormType => MaskType.Joy;
    public override Color FormColor => new Color(1f, 0.9f, 0.4f); // 金色
    public override float MoveSpeedMultiplier => 1.2f;
    public override float DamageMultiplier => 1.0f;
    public override float HealthMultiplier => 1.0f;

    public EnemyJoyForm(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    ////public override void UseSpecialAbility()
    ////{
    ////    // 喜形态技能：治疗光环
    ////    Debug.Log("Joy Form: Healing Aura activated!");
    ////}
}

public class EnemyAngerForm : EnemyBaseForm
{
    public override MaskType FormType => MaskType.Anger;
    public override Color FormColor => new Color(1f, 0.3f, 0.3f); // 红色
    public override float MoveSpeedMultiplier => 0.9f;
    public override float DamageMultiplier => 1.5f;
    public override float HealthMultiplier => 1.2f;

    public EnemyAngerForm(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    ////public override void UseSpecialAbility()
    ////{
    ////    // 怒形态技能：狂暴攻击
    ////    Debug.Log("Anger Form: Berserk activated!");
    ////}
}
// 同理创建 SorrowForm 和 FearForm...

public class EnemySorrowForm : EnemyBaseForm
{
    public EnemySorrowForm(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override MaskType FormType => MaskType.Sorrow;
    public override Color FormColor => new Color();

    public override float MoveSpeedMultiplier => 1f;

    public override float DamageMultiplier => 1f;

    public override float HealthMultiplier => 1f;
}


public class EnemyFearForm : EnemyBaseForm
{
    public EnemyFearForm(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override MaskType FormType => MaskType.Fear;

    public override Color FormColor => new Color();
    public override float MoveSpeedMultiplier => 1f;

    public override float DamageMultiplier => 1f;

    public override float HealthMultiplier => 1f;
}
