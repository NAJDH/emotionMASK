using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject playerProjectilePrefab; // 玩家投射物预制体引用
    [Header("组件引用")]
    private PlayerHitboxManager hitboxManager; // 🟢 新增：只需要这一个引用
    public AnimEvent animEvent;
    [Header("地面检测")]
    public Transform groundCheck;
    public float groundCheckRange = 0.2f;
    public LayerMask groundLayer;

    public static player Instance{get; private set;}
    public Animator anim{get; private set;}
    public Rigidbody2D rb{get; private set;}
    public playerStateMachine stateMachine{get; private set;}
    public playerIdleState idleState{get; private set;}
    public playerMoveState moveState{get; private set;}
    public playerJumpState jumpState{get; private set;}
    public playerAirState airState{get; private set;}
    public playerNormalATK normalATKState{get; private set;}
    public playerTransformState transformState{get; private set;} // 新增形态切换状态
    public playerDieState dieState{get; private set;}
    public playerBeenATKState beenATKState{get; private set;}
    public playerNormalATK2 normalATK2{get; private set;}


    private void Awake()
    {
        hitboxManager = GetComponent<PlayerHitboxManager>(); // 获取攻击判定框管理器组件
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animEvent = GetComponentInChildren<AnimEvent>();
        stateMachine = new playerStateMachine();
        idleState = new playerIdleState(this, stateMachine, "idle");
        moveState = new playerMoveState(this, stateMachine, "move");
        jumpState = new playerJumpState(this, stateMachine, "jump");
        airState  = new playerAirState (this, stateMachine, "jump");
        normalATKState = new playerNormalATK(this, stateMachine, "normalATK");
        transformState = new playerTransformState(this, stateMachine, "transform"); // 初始化形态切换状态
        dieState = new playerDieState(this, stateMachine, "die");
        beenATKState = new playerBeenATKState(this, stateMachine, "beATK");
        normalATK2 = new playerNormalATK2(this, stateMachine, "normalATK2");
        // if(Instance == null)
        //     Instance = this;
        // else
        //     Destroy(gameObject);

    }
    protected void Start() 
    {
        stateMachine.Initialize(idleState);         //这个函数在playerStateMachine里面有写，是初始化第一个状态的
        
        // 重置所有动画事件标志
        animEvent.ResetAnimationEvent();
        animEvent.DisableHitbox();
    }
    protected void Update() 
    {
        stateMachine.currentState.Update();
        Debug.Log($"当前状态：{stateMachine.currentState}");
        playerStateManager.Update(); // 更新形态管理器
        if(playerStateManager.isDead && stateMachine.currentState != dieState)
        {
            stateMachine.ChangeState(dieState);
        }
        if (playerStateManager.isBeHit)
        {
            if(playerStateManager.playerHP > 0 && stateMachine.currentState != beenATKState)
            {
                stateMachine.ChangeState(beenATKState);
            }
            else if(playerStateManager.playerHP <= 0 && stateMachine.currentState != dieState)
            {
                stateMachine.ChangeState(dieState);
            }   
        }
    }

    #region 受伤接口(已注释)
    // //.................................................................................接口
    // //玩家受伤
    // public void TakeDamage(float amount)
    // {
    //     // 这里的代码就是我们之前讨论的：
    //     // 1. 扣血
    //     // 2. 判断死亡
    //     // 3. 播放动画
        
    //     playerStateManager.playerHP -= amount;

    //     if (playerStateManager.isDead)
    //     {
    //         stateMachine.ChangeState(dieState);
    //     }
    //     else
    //     {
    //         stateMachine.ChangeState(beenATKState);
    //     }
    //     Debug.Log($"Player took {amount} damage.");
    // }
    #endregion

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FilpController(xVelocity);
    }
    #region 翻转角色相关参数和函数
    public bool isFacingRight = false;
    private int playerDirection = -1;
    public void Flip()
    {
        playerDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    // 在 player.cs 中添加这个公共方法
public void OnAttackHit(IDamageable target, Collider2D hitInfo)
{
    // 把消息转发给当前状态
    // 这样，如果当前是“普攻状态”，就会触发普攻的逻辑
    //用作伤害计算和特效播放
    stateMachine.currentState.OnAttackHit(target, hitInfo);
}
    public void FilpController(float x)
    {
        if(x > 0 && !isFacingRight) Flip();
        else if(x < 0 && isFacingRight) Flip();
    }
    #endregion
    #region 地面检测
    /// <summary>
    /// 检测是否在地面上
    /// </summary>
    public bool IsGroundDetected() => Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer);

    // 可视化检测范围（仅在编辑器中显示）
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRange);
        }
    }
    #endregion
}