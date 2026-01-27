using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EntityDirection{get; private set;} = 1;
    private bool isFacingRight = true;
    public Animator anim{get; private set;}
    public Rigidbody2D rb{get; private set;}
    protected EnemyStateMachine stateMachine;
    protected void Awake()
    {
        stateMachine = new EnemyStateMachine();
    }
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    protected void Update()
    {
        stateMachine.currentState.Update();
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
    public void SetZeroVelocity()
    {
        // if(isknockback) return;//受击击退的时候不会有其他速度

        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    public void Damage()
    {
        Debug.Log("Enemy Hit!");
    }
}
