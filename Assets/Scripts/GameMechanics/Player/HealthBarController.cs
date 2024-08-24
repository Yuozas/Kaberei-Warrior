using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public Image healthBar;
    public bool CheckIfDead(float damage)
    {
        if (healthBar.fillAmount - damage <= 0)
            return true;
        else
            return false;
    }
    public void MinusHealth(float damage)
    {
        healthBar.fillAmount -= damage;
    }
    public void PlusHealth(float heal)
    {
        if(healthBar.fillAmount >= 1 - heal)
        {
            healthBar.fillAmount = 1;
        }
        else
        {
            healthBar.fillAmount += heal;
        }
    }
}
