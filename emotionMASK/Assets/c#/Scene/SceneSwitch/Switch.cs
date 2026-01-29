using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public void SwitchScene()
    {
        SceneManager.LoadScene(1);
    }
}
