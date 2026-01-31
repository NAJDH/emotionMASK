using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public void PrepareNextLevel() 
    { 
        Debug.Log("Level: 随机选择配置，准备怪物数据..."); 
    }

    public void StartLevel() 
    { 
        Debug.Log("Level: 从池中取出怪物，瞬移到战场！"); 
    }
}