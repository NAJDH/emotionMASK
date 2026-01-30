using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // 引用 TextMeshPro 命名空间
using UnityEngine.SceneManagement; // 引用场景管理命名空间

#region code_original
/* public class DialogueManager : MonoBehaviour
{
    [Header("UI 组件引用")]
    public TextMeshProUGUI nameText;      // 显示名字的文本框
    public TextMeshProUGUI dialogueText;  // 显示内容的文本框
    public Image characterImage;          // 显示立绘的 Image 组件
    public GameObject dialoguePanel;      // 整个对话框的父物体

    [Header("设置")]
    public float typingSpeed = 0.05f;     // 打字速度
    public string nextSceneName;          // 剧情结束后要跳转的场景名字

    // 内部变量
    private Queue<DialogueLine> lines;    // 存储当前对话的所有句子
    private bool isTyping = false;        // 是否正在打字中
    private string currentSentence = "";  // 当前完整句子的缓存

    void Awake()
    {
        lines = new Queue<DialogueLine>();
    }

    // --- 供外部调用的开始方法 ---
    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        dialoguePanel.SetActive(true); // 确保对话框显示
        lines.Clear();

        // 将所有对话加入队列
        foreach (DialogueLine line in dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    // --- 每帧检测点击 ---
    void Update()
    {
        // 检测鼠标左键点击 或 触摸屏幕
        if (Input.GetMouseButtonDown(0)) 
        {
            if (isTyping)
            {
                // 如果正在打字，点击则瞬间显示全句 (跳过打字效果)
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                // 如果没在打字，点击则播放下一句
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        // 如果队列为空，说明对话结束
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();

        // 设置名字
        nameText.text = line.characterName;

        // 设置立绘 (如果有图片则显示，没有则隐藏或保持不变，视需求而定)
        if(line.characterSprite != null)
        {
            characterImage.sprite = line.characterSprite;
            characterImage.gameObject.SetActive(true);
        }
        else
        {
            // 如果这句没有配图，可以选择隐藏 Image
            // characterImage.gameObject.SetActive(false); 
        }

        // 启动打字机效果
        currentSentence = line.sentence; // 缓存全句
        StopAllCoroutines(); // 停止上一句可能未完成的协程
        StartCoroutine(TypeSentence(line.sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = ""; // 清空文本框

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        Debug.Log("剧情结束，切换场景...");
        // 可以在这里加个简单的黑屏淡出动画再切换
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    } 
}*/
#endregion



public class DialogueManager : MonoBehaviour
{
    [Header("UI 组件引用")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image leftImage;   // 左边的立绘槽位
    public Image rightImage;  // 右边的立绘槽位
    public GameObject dialoguePanel;


    // --- 新增：转场幕布 ---
    [Header("转场设置")]
    public Image fadePanel;       // 拖入刚才创建的 FadePanel
    public float fadeDuration = 1.0f; // 转场变黑需要几秒


    [Header("设置")]
    public float typingSpeed = 0.05f;
    public string nextSceneName;

    // 用于让非当前说话角色的立绘变暗 (颜色变灰)
    private Color activeColor = Color.white;
    private Color inactiveColor = new Color(0.5f, 0.5f, 0.5f, 1f); 

    private Queue<DialogueLine> lines;
    private bool isTyping = false;
    private string currentSentence = "";


    // --- 新增：防止转场时点击 ---
    private bool isTransitioning = false;
    

    void Awake()
    {
        lines = new Queue<DialogueLine>();
    }

    // 这里参数改成了 StoryChapter，接收一整个章节
    public void StartDialogue(StoryChapter story)
    {
        dialoguePanel.SetActive(true);
        lines.Clear();

        // 重置图片显示（可选：开始时隐藏两边，或者显示默认图）
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);

        foreach (DialogueLine line in story.lines)
        {
            lines.Enqueue(line);
        }

        DisplayNextSentence();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = currentSentence;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();

        // 1. 设置文本
        nameText.text = line.characterName;
        currentSentence = line.sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(line.sentence));

        // 2. 处理立绘逻辑 (核心修改)
        UpdatePortraits(line);
    }

    // 专门处理立绘显示和变暗的逻辑
    void UpdatePortraits(DialogueLine line)
    {
        Image currentImage = null;   // 当前说话人的图片组件
        Image otherImage = null;     // 听话人的图片组件

        // 判断左右
        if (line.position == CharacterPosition.Left)
        {
            currentImage = leftImage;
            otherImage = rightImage;
        }
        else
        {
            currentImage = rightImage;
            otherImage = leftImage;
        }

        // 激活当前立绘组件
        currentImage.gameObject.SetActive(true);

        // 如果配置了新的图片，就替换；如果为空，就保持上一张图
        if (line.characterSprite != null)
        {
            currentImage.sprite = line.characterSprite;
            // NativeSize 保证图片比例正确，视情况开启
            // currentImage.SetNativeSize(); 
        }

        // 视觉效果：当前说话人全亮，另一人变暗
        currentImage.color = activeColor;
        
        // 只有当另一边有图片显示时，才将其变暗
        if (otherImage.gameObject.activeSelf)
        {
            otherImage.color = inactiveColor;
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }


    //直接转场
    /* void EndDialogue()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    } */



    // --- 修改：不再直接切场景，而是调用协程 ---
    void EndDialogue()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            StartCoroutine(FadeAndLoadScene());
        }
        else
        {
            Debug.Log("剧情结束，但没有设置 Next Scene Name");
        }
    }

    // --- 新增：核心转场逻辑 ---
    IEnumerator FadeAndLoadScene()
    {
        isTransitioning = true; // 锁定输入
        Debug.Log("开始淡出转场...");

        float timer = 0f;
        
        // 确保 Panel 是开启的
        fadePanel.gameObject.SetActive(true);

        // 循环增加 Alpha 值，直到变成 1 (全黑)
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration; // 计算 0 到 1 的比例
            fadePanel.color = new Color(0, 0, 0, alpha);
            yield return null; // 等待下一帧
        }

        // 确保最后一定是完全黑的
        fadePanel.color = new Color(0, 0, 0, 1);

        // 等待一小会儿让玩家反应过来 (可选)
        yield return new WaitForSeconds(0.5f);

        Debug.Log("屏幕全黑，加载场景：" + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }

}