using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float health, float value)
    {
        slider.maxValue = health;
        slider.value = value;
    }
    public void SetHealth(float health) 
    {
        slider.value = health;
    }
    public void PlusHealth(float health) 
    {
        slider.value += health;
    }
    public void MinusHealth(float health) 
    {
        slider.value -= health;
    }
    public float GetValue()
    {
        return slider.value;
    }
}
