using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // 玩家远程攻击的投射物预制体（用于生成子弹/特效等）
    public GameObject playerProjectilePrefab;

    [Header("组件引用")]
    // 负责管理玩家攻击判定框的组件（Hitbox 开关/定位等）
    private PlayerHitboxManager hitboxManager;
    // 动画事件脚本（用于动画帧回调）
    public AnimEvent animEvent;

    [Header("地面检测")]
    // 地面检测的参考点（脚底位置）
    public Transform groundCheck;
    // 地面检测范围（OverlapCircle 的半径）
    public float groundCheckRange = 0.2f;
    // 地面图层（用于过滤哪些碰撞体算“地面”）
    public LayerMask groundLayer;

    [Header("输入缓冲")]
    // 输入缓冲窗口（秒），在窗口内按键可在合适时机触发
    public float inputBufferTime = 0.15f;
    // 最近一次“跳跃键”按下的时间
    private float jumpPressedTime = -999f;
    // 最近一次“普攻1键”按下的时间
    private float atk1PressedTime = -999f;
    // 最近一次“普攻2键”按下的时间
    private float atk2PressedTime = -999f;

    // 单例实例（如果你需要全局访问玩家）
    public static player Instance { get; private set; }

    // 动画控制器
    public Animator anim { get; private set; }
    // 刚体组件（控制物理运动）
    public Rigidbody2D rb { get; private set; }

    // 状态机与各类状态实例
    public playerStateMachine stateMachine { get; private set; }
    public playerIdleState idleState { get; private set; }
    public playerMoveState moveState { get; private set; }
    public playerJumpState jumpState { get; private set; }
    public playerAirState airState { get; private set; }
    public playerNormalATK normalATKState { get; private set; }
    public playerTransformState transformState { get; private set; }
    public playerDieState dieState { get; private set; }
    public playerBeenATKState beenATKState { get; private set; }
    public playerNormalATK2 normalATK2 { get; private set; }

    private void Awake()
    {
        // 获取必要组件
        hitboxManager = GetComponent<PlayerHitboxManager>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        animEvent = GetComponentInChildren<AnimEvent>();

        // 初始化状态机与各状态实例
        stateMachine = new playerStateMachine();
        idleState = new playerIdleState(this, stateMachine, "idle");
        moveState = new playerMoveState(this, stateMachine, "move");
        jumpState = new playerJumpState(this, stateMachine, "jump");
        airState  = new playerAirState (this, stateMachine, "jump");
        normalATKState = new playerNormalATK(this, stateMachine, "normalATK");
        transformState = new playerTransformState(this, stateMachine, "transform");
        dieState = new playerDieState(this, stateMachine, "die");
        beenATKState = new playerBeenATKState(this, stateMachine, "beATK");
        normalATK2 = new playerNormalATK2(this, stateMachine, "normalATK2");

        // 如果需要单例，可以取消注释
        // if(Instance == null)
        //     Instance = this;
        // else
        //     Destroy(gameObject);
    }

    protected void Start()
    {
        // 初始化状态机的起始状态（通常是 Idle）
        stateMachine.Initialize(idleState);

        // 重置动画事件标记，避免动画事件残留
        animEvent.ResetAnimationEvent();
        animEvent.DisableHitbox();
    }

    protected void Update()
    {
        // 采集输入并写入缓冲（按键记录）
        CaptureInputBuffer();

        // 更新当前状态（状态内部会处理逻辑/移动/动画）
        stateMachine.currentState.Update();
        Debug.Log($"当前状态：{stateMachine.currentState}");

        // 形态管理器更新（需要先确认 PlayerFormManager 已初始化）
        if (PlayerFormManager.playerForm != null)
            playerStateManager.Update();

        // 根据状态管理器的标志触发死亡/受击切换
        if (playerStateManager.isDead && stateMachine.currentState != dieState)
        {
            stateMachine.ChangeState(dieState);
        }

        if (playerStateManager.isBeHit)
        {
            if (playerStateManager.playerHP > 0 && stateMachine.currentState != beenATKState)
            {
                stateMachine.ChangeState(beenATKState);
            }
            else if (playerStateManager.playerHP <= 0 && stateMachine.currentState != dieState)
            {
                stateMachine.ChangeState(dieState);
            }
        }
    }

    /// <summary>
    /// 记录按键输入的时间点，用于输入缓冲
    /// </summary>
    private void CaptureInputBuffer()
    {
        if (Input.GetKeyDown(KeyCode.Space)) jumpPressedTime = Time.time;
        if (Input.GetKeyDown(KeyCode.Mouse0)) atk1PressedTime = Time.time;
        if (Input.GetKeyDown(KeyCode.Mouse1)) atk2PressedTime = Time.time;
    }

    /// <summary>
    /// 消费跳跃缓冲：在缓冲窗口内返回 true，并清空时间
    /// </summary>
    public bool ConsumeBufferedJump()
    {
        if (Time.time - jumpPressedTime <= inputBufferTime)
        {
            jumpPressedTime = -999f;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 消费普攻1缓冲
    /// </summary>
    public bool ConsumeBufferedAtk1()
    {
        if (Time.time - atk1PressedTime <= inputBufferTime)
        {
            atk1PressedTime = -999f;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 消费普攻2缓冲
    /// </summary>
    public bool ConsumeBufferedAtk2()
    {
        if (Time.time - atk2PressedTime <= inputBufferTime)
        {
            atk2PressedTime = -999f;
            return true;
        }
        return false;
    }

    #region 受伤接口(已注释)
    // 示例：受伤处理逻辑（扣血、切状态）
    // public void TakeDamage(float amount)
    // {
    //     playerStateManager.playerHP -= amount;
    //     if (playerStateManager.isDead)
    //         stateMachine.ChangeState(dieState);
    //     else
    //         stateMachine.ChangeState(beenATKState);
    //     Debug.Log($"Player took {amount} damage.");
    // }
    #endregion

    /// <summary>
    /// 设置刚体速度，并处理朝向翻转
    /// </summary>
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
        FilpController(xVelocity);
    }

    #region 翻转角色相关参数和函数
    // 当前朝向（true=右，false=左）
    public bool isFacingRight = false;
    // 方向值（1 或 -1）
    private int playerDirection = -1;

    /// <summary>
    /// 翻转角色朝向（绕 Y 轴旋转 180°）
    /// </summary>
    public void Flip()
    {
        playerDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    /// <summary>
    /// 攻击命中时转发给当前状态（由状态决定具体处理）
    /// </summary>
    public void OnAttackHit(IDamageable target, Collider2D hitInfo)
    {
        stateMachine.currentState.OnAttackHit(target, hitInfo);
    }

    /// <summary>
    /// 根据水平速度自动翻转朝向
    /// </summary>
    public void FilpController(float x)
    {
        if (x > 0 && !isFacingRight) Flip();
        else if (x < 0 && isFacingRight) Flip();
    }
    #endregion

    #region 地面检测
    /// <summary>
    /// 检测角色是否在地面上
    /// </summary>
    public bool IsGroundDetected() =>
        Physics2D.OverlapCircle(groundCheck.position, groundCheckRange, groundLayer);

    /// <summary>
    /// 在编辑器中可视化地面检测范围
    /// </summary>
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