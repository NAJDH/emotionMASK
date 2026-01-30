using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerFormManager : MonoBehaviour
{
    public static PlayerFormManager playerForm { get; private set; }
    
    [Header("四种玩家形态")]
    public GameObject form1; // 形态1
    public GameObject form2; // 形态2
    public GameObject form3; // 形态3
    public GameObject form4; // 形态4
    
    [Header("当前形态")]
    public int currentFormIndex = 1; // 当前形态索引 (1-4)
    
    private GameObject currentPlayerForm; // 当前控制的形态对象
    private Dictionary<int, GameObject> formDictionary; // 形态字典
    // 【第二步：添加摄像机引用】
    [Header("摄像机设置")]
    public CinemachineVirtualCamera activeCamera; // 记得在 Inspector 里把你的 CM vcam1 拖进去
    
    private void Awake()
    {
        if (playerForm == null)
            playerForm = this;
        else
            Destroy(gameObject);
            
        InitializeForms();
    }
    
    private void InitializeForms()
    {
        // 初始化形态字典
        formDictionary = new Dictionary<int, GameObject>
        {
            { 1, form1 },
            { 2, form2 },
            { 3, form3 },
            { 4, form4 }
        };
        
        // **关键修改：先激活所有形态，让它们完成初始化**
        foreach (var form in formDictionary.Values)
        {
            if (form != null)
            {
                form.SetActive(true);
            }
        }
        
        // 等待一帧后再隐藏
        StartCoroutine(InitializeFormsDelayed());
    }
    
    private IEnumerator InitializeFormsDelayed()
    {
        yield return null; // 等待一帧，让所有Awake和Start执行
        
        // 设置初始形态
        currentPlayerForm = formDictionary[currentFormIndex];
        
        // 隐藏其他形态
        foreach (var form in formDictionary.Values)
        {
            if (form != null && form != currentPlayerForm)
            {
                form.SetActive(false);
            }
        }
    }
    
    private void Update()
    {
        // 检测按键 1-4
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TryTransform(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TryTransform(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TryTransform(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TryTransform(4);
        }
    }
    
    private void TryTransform(int targetFormIndex)
    {
        // 如果按的是当前形态的按键，不做任何操作
        if (targetFormIndex == currentFormIndex)
        {
            Debug.Log($"已经是形态{targetFormIndex}，无需切换");
            return;
        }
        
        // 检查目标形态是否存在
        if (!formDictionary.ContainsKey(targetFormIndex) || formDictionary[targetFormIndex] == null)
        {
            Debug.LogWarning($"形态{targetFormIndex}不存在！");
            return;
        }
        
        // 触发形态切换
        GameObject targetForm = formDictionary[targetFormIndex];
        
        player currentPlayer = currentPlayerForm.GetComponent<player>();
        if (currentPlayer != null && currentPlayer.transformState != null)
        {
            currentPlayer.transformState.SetTargetForm(targetForm, targetFormIndex);
            currentPlayer.stateMachine.ChangeState(currentPlayer.transformState);
        }
    }
    
    public void SwitchControl(GameObject newForm, int newFormIndex)
    {
        // 禁用当前形态 - 改为隐藏整个物体
        if (currentPlayerForm != null)
        {
            currentPlayerForm.SetActive(false);
        }
        
        // 交换位置
        Vector3 tempPosition = currentPlayerForm.transform.position;
        currentPlayerForm.transform.position = newForm.transform.position;
        newForm.transform.position = tempPosition;
        
        // 更新当前形态
        currentPlayerForm = newForm;
        currentFormIndex = newFormIndex;
        
        // 启用新形态 - 改为激活整个物体
        currentPlayerForm.SetActive(true);
        
        // 延迟切换状态，确保Awake已执行
        StartCoroutine(DelayedStateChange());
        
        Debug.Log($"已切换到形态{newFormIndex}");
    }
    
    private IEnumerator DelayedStateChange()
    {
        yield return null; // 只需等待一帧
        
        player newPlayer = currentPlayerForm.GetComponent<player>();
        if (newPlayer != null)
        {
            // 因为已经预初始化过，所以状态机应该已经存在
            if (newPlayer.stateMachine != null && newPlayer.idleState != null)
            {
                // 如果当前状态为空，先初始化
                if (newPlayer.stateMachine.currentState == null)
                {
                    newPlayer.stateMachine.Initialize(newPlayer.idleState);
                }
                else
                {
                    newPlayer.stateMachine.ChangeState(newPlayer.idleState);
                }
                Debug.Log($"形态{currentFormIndex}状态切换完成");
            }
            else
            {
                Debug.LogError($"形态{currentFormIndex}的状态机或idleState为空！");
            }
        }
        
        UpdateCameraTarget();
    }
    
    private void UpdateCameraTarget()
    {
        // 如果你有摄像机跟随脚本，在这里更新目标
        // 例如: CameraFollow.Instance.SetTarget(currentPlayerForm.transform);
        if (activeCamera != null && currentPlayerForm != null)
        {
            // 告诉摄像机：现在的跟随目标是当前的主角
            activeCamera.Follow = currentPlayerForm.transform;

            // 如果你的游戏需要 LookAt (通常 3D 需要，2D 不需要)，把下面这行取消注释
            // activeCamera.LookAt = currentPlayerForm.transform; 
            
            Debug.Log($"摄像机已跟随: {currentPlayerForm.name}");
        }
        else
        {
            Debug.LogWarning("摄像机或当前角色为空，无法跟随！");
        }
    }
}