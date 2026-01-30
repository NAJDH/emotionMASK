using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour, IDamageable
{
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


    private void Awake()
    {
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
        // 单例模式
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
    }
    public void TakeDamage(float amount, MaskType attackerMask)
    {
        Debug.Log($"Player took {amount} damage.");
    }


    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FilpController(xVelocity);
    }
    #region 翻转角色相关参数和函数
    private bool isFacingRight = false;
    private int playerDirection = -1;
    public void Flip()
    {
        playerDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
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