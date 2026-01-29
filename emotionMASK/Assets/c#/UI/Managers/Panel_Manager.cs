using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 面板管理器，用栈管理UI
/// </summary>
public class Panel_Manager
{
    //存储UI的栈
    private Stack<BasePanel> stackPanel;

    private UI_Manager uiManager;
    private BasePanel panel;

    public Panel_Manager()
    {
        stackPanel = new Stack<BasePanel>();
        uiManager = new UI_Manager();
    }

    //UI的入栈操作，显示一个面板
    public void Push(BasePanel  nextPanel)
    {
        if(stackPanel.Count > 0)
        {
            panel = stackPanel.Peek();
            panel.Pause();
        }
        stackPanel.Push(nextPanel);

        GameObject panelGo = uiManager.GetSingleType(nextPanel.uiType);
        nextPanel.Initialize(new UI_Tool(panelGo));
        nextPanel.Initialize(this);
        nextPanel.Initialize(uiManager);
        nextPanel.Enter();
    }

    //执行面板的出栈操作
    public void Pop()
    {
        if (stackPanel.Count > 0)
        {
            stackPanel.Peek().Exit();
            stackPanel.Pop();
        }
        if (stackPanel.Count > 0)
            stackPanel.Peek().Resume();
    }
}
