using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFormManager : MonoBehaviour
{
    public static PlayerFormManager Instance { get; private set; }
    
    [Header("四种玩家形态")]
    public GameObject form1; // 形态1
    public GameObject form2; // 形态2
    public GameObject form3; // 形态3
    public GameObject form4; // 形态4
    
    [Header("当前形态")]
    public int currentFormIndex = 1; // 当前形态索引 (1-4)
    
    private GameObject currentPlayerForm; // 当前控制的形态对象
    private Dictionary<int, GameObject> formDictionary; // 形态字典
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
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
        
        // 设置初始形态
        currentPlayerForm = formDictionary[currentFormIndex];
        
        // 禁用其他形态的控制
        foreach (var form in formDictionary.Values)
        {
            if (form != null && form != currentPlayerForm)
            {
                player p = form.GetComponent<player>();
                if (p != null) p.enabled = false;
            }
        }
        
        // 启用当前形态
        if (currentPlayerForm != null)
        {
            player currentPlayer = currentPlayerForm.GetComponent<player>();
            if (currentPlayer != null) currentPlayer.enabled = true;
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
        // 禁用当前形态的控制
        if (currentPlayerForm != null)
        {
            player oldPlayer = currentPlayerForm.GetComponent<player>();
            if (oldPlayer != null)
            {
                oldPlayer.enabled = false;
            }
        }
        
        // 交换位置
        Vector3 tempPosition = currentPlayerForm.transform.position;
        currentPlayerForm.transform.position = newForm.transform.position;
        newForm.transform.position = tempPosition;
        
        // 更新当前形态
        currentPlayerForm = newForm;
        currentFormIndex = newFormIndex;
        
        // 启用新形态的控制
        player newPlayer = currentPlayerForm.GetComponent<player>();
        if (newPlayer != null)
        {
            newPlayer.enabled = true;
        }
        
        // 更新摄像机跟随目标（如果有）
        UpdateCameraTarget();
        
        Debug.Log($"已切换到形态{newFormIndex}");
    }
    
    private void UpdateCameraTarget()
    {
        // 如果你有摄像机跟随脚本，在这里更新目标
        // 例如: CameraFollow.Instance.SetTarget(currentPlayerForm.transform);
    }
}