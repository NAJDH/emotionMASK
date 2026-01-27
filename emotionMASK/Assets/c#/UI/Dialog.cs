using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] public Text dialogBoxText;
    public string dialogText;
    [SerializeField]private float dialogTimer;
    private float showTimer;

    private void Start()
    {
        dialogBox.SetActive(false);
        showTimer = dialogTimer;
    }

    private void Update()
    {
        showTimer -= Time.deltaTime;

        if (showTimer <= 0)
            dialogBox.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Show the dialog-box.");
            dialogBoxText.text = dialogText;
            dialogBox.SetActive(true);
            showTimer = dialogTimer;
        }

    }
}
