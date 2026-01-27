using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFormManager : MonoBehaviour
{
    public static PlayerFormManager Instance { get; private set; }
    
    [Header("玩家形态")]
    public GameObject currentPlayerForm; // 当前控制的形态
    public GameObject alternateForm; // 另一个形态
    
    [Header("切换设置")]
    public KeyCode transformKey = KeyCode.T; // 切换按键
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(transformKey))
        {
            TriggerTransform();
        }
    }
    
    private void TriggerTransform()
    {
        if (currentPlayerForm != null)
        {
            player currentPlayer = currentPlayerForm.GetComponent<player>();
            if (currentPlayer != null && currentPlayer.transformState != null)
            {
                currentPlayer.transformState.SetTargetForm(alternateForm);
                currentPlayer.stateMachine.ChangeState(currentPlayer.transformState);
            }
        }
    }
    
    public void SwitchControl(GameObject newForm)
    {
        // 禁用当前形态的控制
        if (currentPlayerForm != null)
        {
            player oldPlayer = currentPlayerForm.GetComponent<player>();
            if (oldPlayer != null)
            {
                oldPlayer.enabled = false;
            }
        }
        
        // 启用新形态的控制
        GameObject temp = currentPlayerForm;
        currentPlayerForm = newForm;
        alternateForm = temp;
        
        player newPlayer = currentPlayerForm.GetComponent<player>();
        if (newPlayer != null)
        {
            newPlayer.enabled = true;
        }
        
        // 更新摄像机跟随目标（如果有）
        UpdateCameraTarget();
    }
    
    private void UpdateCameraTarget()
    {
        // 如果你有摄像机跟随脚本，在这里更新目标
        // 例如: CameraFollow.Instance.SetTarget(currentPlayerForm.transform);
    }
}