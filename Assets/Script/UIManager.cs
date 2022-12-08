using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TMP_Text scoreText;
    public TMP_Text blockText;
    public TMP_Text counterText;

    public Slider healthBarSlider;
    public TMP_Text healthBarText;


    public void SetMaxHealth(int _health)
    {
        healthBarSlider.maxValue = _health;
        healthBarSlider.value = _health;
    }

    public void UpdateHealthBar(int _health)
    {
        healthBarSlider.value = _health;
        healthBarText.text = _health.ToString();
    }

    public void UpdateScore(int _score)
    {
        scoreText.text = "Score: " + _score;
    }

    public void UpdateBlockCounter(int _bcounter)
    {
        blockText.text = "Block : " + _bcounter;
    }
    
    public void UpdateCounterCounter(int _ccounter)
    {
        counterText.text = "Counter : " + _ccounter;
    }
}
