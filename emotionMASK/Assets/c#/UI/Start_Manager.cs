using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_Manager : MonoBehaviour
{
    Panel_Manager panelManager;


    private void Awake()
    {
        panelManager = new Panel_Manager();
    }

    // Start is called before the first frame update
    void Start()
    {
        panelManager.Push(new StartPanel());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
