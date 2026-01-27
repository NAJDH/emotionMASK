using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    public int EntityDirection{get; private set;} = 1;
    private bool isFacingRight = true;
    public Animator anim{get; private set;}
    public Rigidbody2D rb{get; private set;}
    protected EnemyStateMachine stateMachine;

    [Header("移动参数")]
    public float moveSpeed = 6f;
    public float idleTime;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;

    //physics
    [Header("物理检测")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance;
    public bool isGrounded;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private float wallCheckDistance;
    public bool isTouchingTheWall;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    //state
    public Enemy_IdleState idleState { get; private set;}
    public Enemy_MoveState moveState { get; private set;}


    protected void Awake()
    {
        stateMachine = new EnemyStateMachine();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
    }
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

        stateMachine.Initialize(idleState);
    }
    protected void Update()
    {
        stateMachine.currentState.Update();

        PhysicsCheck();
    }

    public void TakeDamage(float amount, MaskType attackerMask)
    {
        Debug.Log($"Enemy took {amount} damage from {attackerMask} mask.");
    }
    #region 翻转函数
    public void Flip()
    {
        EntityDirection *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    public void FilpController(float x)
    {
        if(x > 0 && !isFacingRight) Flip();
        else if(x < 0 && isFacingRight) Flip();
    }
    #endregion

    public void SetVelocity(float x, float y)
    {
        rb.velocity = new Vector2(x, y);
    }

    public void SetZeroVelocity()
    {
        // if(isknockback) return;//受击击退的时候不会有其他速度

        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public void Damage()
    {
        Debug.Log("Enemy Hit!");
    }

    private void PhysicsCheck()
    {
        isGrounded = Physics2D.Raycast(groundCheckPoint.position, Vector3.down, groundCheckDistance);
        isTouchingTheWall = Physics2D.Raycast(wallCheckPoint.position, Vector3.right * EntityDirection, wallCheckDistance, wallLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + new Vector3(wallCheckDistance * EntityDirection, 0, 0));
    }
}
