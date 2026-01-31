using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image hateValue;
    public Image hateBarFrame;
    private float currentHateValue;

    private void Update()
    {
        CalculateTheHateValue();
        hateValue.fillAmount = currentHateValue;
    }


    private void CalculateTheHateValue()
    {
        currentHateValue = (playerStateManager.maxPlayerHP - playerStateManager.playerHP) / playerStateManager.maxPlayerHP;
    }
}
