using UnityEngine;

#region OldCode
/* public class DialogueTrigger : MonoBehaviour
{
    [Header("剧情内容配置")]
    public DialogueLine[] dialogueLines; // 在 Inspector 中配置每一句话

    private DialogueManager manager;

    void Start()
    {
        // 查找场景中的管理器
        manager = FindObjectOfType<DialogueManager>();
        
        // 游戏一开始就自动开始剧情
        if (manager != null)
        {
            manager.StartDialogue(dialogueLines);
        }
        else 
        {
            Debug.LogError("场景中没有找到 DialogueManager！");
        }
    }
} */
#endregion



using System.Collections.Generic;


public class RandomStoryTrigger : MonoBehaviour
{
    [Header("剧情库")]
    // 这里是一个列表，可以放 Story A, Story B, Story C...
    public List<StoryChapter> allStories; 

    private DialogueManager manager;

    void Start()
    {
        manager = FindObjectOfType<DialogueManager>();

        if (manager != null && allStories.Count > 0)
        {
            // --- 核心逻辑：随机抽取 ---
            int randomIndex = Random.Range(0, allStories.Count);
            StoryChapter selectedStory = allStories[randomIndex];

            Debug.Log($"随机选中了剧情: {selectedStory.chapterName}");

            // 把选中的那个剧本传给管理器
            manager.StartDialogue(selectedStory);
        }
        else
        {
            Debug.LogWarning("没有找到 DialogueManager 或者 剧情列表为空！");
        }
    }
}