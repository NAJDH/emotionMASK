using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    //public Animator childTran;

    public float transitionTime = 1f;

    private void Awake()
    {
        transition = GetComponent<Animator>();
        //childTran = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            LoadNextScene();
    }

    public void LoadNextScene()
    {
        StartCoroutine(LoadSceneCO(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadSceneCO(int sceneIndex)
    {
        //childTran.SetTrigger("Start");
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
