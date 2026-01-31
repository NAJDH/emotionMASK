using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour,IDamageable
{
    public int EntityDirection{get; private set;} = 1;
    public bool isFacingRight { get; private set; } = true;
    public SpriteRenderer spriteRenderer {  get; private set; }
    public Animator anim{get; private set;}
    public Rigidbody2D rb{get; private set;}
    protected EnemyStateMachine stateMachine;

    [Header("基本属性")]
    public float currentHealth;
    public float maxHealth;
    public float damageValue;


    [Header("移动参数")]
    public float moveSpeed = 6f;
    public float idleTime;
    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1;


    #region 切形态相关变量
    //[Header("形态系统")]
    //public MaskType startingForm = MaskType.Joy;
    //[SerializeField] private float transformCooldown = 5f;
    //[SerializeField] private float healthThresholdForTransform = 0.3f;

    //// 形态属性
    //[System.Serializable]
    //public class FormVisuals
    //{
    //    public MaskType formType;
    //    public GameObject formModel; // 不同形态的模型（可选）
    //    public ParticleSystem transformEffect;
    //    public AudioClip transformSound;
    //}
    //public FormVisuals[] formVisuals;

    //// 当前形态相关
    //private MaskType currentForm;
    //public MaskType CurrentForm => currentForm;
    //private float lastTransformTime;
    //private EnemyBaseForm currentFormInstance;
    //private Dictionary<MaskType, EnemyBaseForm> availableForms = new Dictionary<MaskType, EnemyBaseForm>();
    #endregion

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

    [Header("玩家检测")]
    [SerializeField] private Transform playerCheckPoint;
    [SerializeField] private float playerCheckDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("交战相关")]
    public float battleMoveSpeed;
    public float attackDistance;
    public float battleLastDuration;
    public float minRetreatDistance;
    public Vector2 retreatVelocity;
    private bool isDead;

    [Header("检测目标")]
    [SerializeField] private Transform targetCheckPoint;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;


    //states
    public Enemy_IdleState idleState { get; private set;}
    public Enemy_MoveState moveState { get; private set; }
    public Enemy_AttackState attackState { get; private set; }
    // public Enemy_TransformState transformState { get; private set;}
    public Enemy_BattleState battleState { get; private set;}


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        stateMachine = new EnemyStateMachine();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        
        #region
        //transformState = new Enemy_TransformState(this, stateMachine, "transform");

        //// 初始化所有形态
        //InitializeForms();
        #endregion
    }
    protected virtual void Start()
    {
        currentHealth = maxHealth;

        #region
        //// 设置初始形态
        //TransformTo(startingForm);
        #endregion

        stateMachine.Initialize(idleState);
    }
    protected virtual void Update()
    {
        stateMachine.currentState.Update();

        PhysicsCheck();

        

        #region
        //// 形态切换逻辑
        //UpdateFormLogic();
        #endregion
    }


    #region
    //private void InitializeForms()
    //{
    //    // 注册所有形态
    //    availableForms.Add(MaskType.Joy, new EnemyJoyForm(this, stateMachine));
    //    availableForms.Add(MaskType.Anger, new EnemyAngerForm(this, stateMachine));
    //    // 添加更多形态...
    //}

    //private void UpdateFormLogic()
    //{
    //    // 定时变换
    //    if (Time.time > lastTransformTime + transformCooldown)
    //    {
    //        TryRandomTransform();
    //    }
    //}

    //// 形态切换核心方法
    //public void TransformTo(MaskType newForm)
    //{
    //    if (currentForm == newForm) return;
    //    if (Time.time < lastTransformTime + transformCooldown) return;

    //    Debug.Log($"Enemy transforming from {currentForm} to {newForm}");

    //    // 进入变换状态
    //    stateMachine.ChangeState(transformState);

    //    // 设置变换目标
    //    transformState.SetTargetForm(newForm);

    //    lastTransformTime = Time.time;
    //}

    //// 实际完成变换（由动画事件调用）
    //public void CompleteTransform(MaskType newForm)
    //{
    //    // 更新当前形态
    //    currentForm = newForm;

    //    // 应用新形态属性
    //    if (availableForms.ContainsKey(newForm))
    //    {
    //        currentFormInstance = availableForms[newForm];
    //        currentFormInstance.EnterForm();
    //    }

    //    // 播放变换效果
    //    PlayTransformEffect(newForm);

    //    // 返回空闲状态
    //    stateMachine.ChangeState(idleState);
    //}

    //private void PlayTransformEffect(MaskType form)
    //{
    //    // 查找对应的视觉效果
    //    foreach (var visual in formVisuals)
    //    {
    //        if (visual.formType == form)
    //        {
    //            if (visual.transformEffect != null)
    //            {
    //                Instantiate(visual.transformEffect, transform.position, Quaternion.identity);
    //            }
    //            // 播放音效
    //            // AudioManager.PlaySound(visual.transformSound);
    //            break;
    //        }
    //    }
    //}

    //// 随机变换
    //private void TryRandomTransform()
    //{
    //    // 排除当前形态，从其他中随机选择
    //    List<MaskType> availableForms = new List<MaskType>();
    //    foreach (MaskType type in System.Enum.GetValues(typeof(MaskType)))
    //    {
    //        if (type != currentForm)
    //            availableForms.Add(type);
    //    }

    //    if (availableForms.Count > 0)
    //    {
    //        MaskType randomForm = availableForms[Random.Range(0, availableForms.Count)];
    //        TransformTo(randomForm);
    //    }
    //}
    #endregion

    //受击相关
    public virtual void TakeDamage(float damage)
    {
        Debug.Log("打到敌人了！！！！！！！！！！！！！！！！！！！！！！！！！！");
        
        if (isDead)
            return;
        
        ReduceHP(damage);
    }
    protected void ReduceHP(float amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
            Die();
    }
    protected void Die()
    {
        isDead = true;
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

    public void MovementOver()  //结束动作（如攻击）
    {
        stateMachine.currentState.MovementOver();
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


    public RaycastHit2D PlayerDetected()  //检测视线内的玩家
    {
        // 我们先用一个掩码把 玩家 和 障碍（墙） 一起检测，以便墙可以阻挡视线
        RaycastHit2D hit = Physics2D.Raycast(playerCheckPoint.position, Vector3.right * EntityDirection, playerCheckDistance,
            whatIsPlayer | wallLayer);

        // 没有任何命中
        if (hit.collider == null)
        {
            Debug.Log("Can't find anyone!!");
            return default;
        }

        // 如果命中的是玩家层，且该对象是激活的，则返回命中结果
        int hitLayer = hit.collider.gameObject.layer;
        int playerLayer = LayerMask.NameToLayer("Player");
        //int wallLayerIndex = -1;
        // 获取 wallLayer 索引（如果需要判断）
        // 注意：上面 raycast mask 已包含 wallLayer，所以若命中为墙则认为被挡住
        if (hitLayer == playerLayer)
        {
            if (!hit.collider.gameObject.activeInHierarchy)
            {
                // 已被禁用的玩家对象，不视为命中
                return default;
            }

            return hit;
        }
        else
        {
            // 如果命中的是墙或其他东西，则视为被挡住
            Debug.Log("Player view blocked by wall or other: " + hit.collider.name);
            return default;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheckPoint.position, targetCheckRadius);

        Gizmos.DrawLine(groundCheckPoint.position, groundCheckPoint.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawLine(wallCheckPoint.position, wallCheckPoint.position + new Vector3(wallCheckDistance * EntityDirection, 0, 0));
       
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheckPoint.position, playerCheckPoint.position + new Vector3(playerCheckDistance * EntityDirection, 0, 0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheckPoint.position, playerCheckPoint.position + new Vector3(attackDistance * EntityDirection, 0, 0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheckPoint.position, playerCheckPoint.position + new Vector3(minRetreatDistance * EntityDirection, 0, 0));
    }
}
