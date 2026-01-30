using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    [Header("命中的触发器")]
    public bool hitTriggered = false;

    public bool AnimationTriggered { get; private set; }

    public void TriggerAnimationEvent()
    {
        AnimationTriggered = true;
    }

    public void ResetAnimationEvent()
    {
        AnimationTriggered = false;
    }

    // 在动画关键帧调用：开启判定
    public void EnableHitbox()
    {
        hitTriggered = true;
        Debug.Log("碰撞框开启");
    }
    

    // 在动画关键帧调用：关闭判定
    public void DisableHitbox()
    {
        hitTriggered = false;
        Debug.Log("碰撞框关闭");
    }
    public void TriggerTransformComplete()
{
    // player playerScript = GetComponentInParent<player>();
    // if (playerScript != null && playerScript.transformState != null)
    // {
    //     playerScript.transformState.CompleteTransform();
    // }
    player playerScript = GetComponentInParent<player>();
    if (playerScript != null)
    {
        playerScript.transformState.CompleteTransform();
    }
}
    // public void ceshi()
    // {
    //     //这个是用来测试顿帧，镜头震动和音效的动画事件
    //     HitStopManager.Instance.PlayHitStop();
        
    // }
}
