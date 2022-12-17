using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public float currentScore;
    public float bestScore;
    
    public TMP_Text scoreText;
    public TMP_Text blockText;
    public TMP_Text counterText;

    public Slider healthBarSlider;
    public TMP_Text healthBarText;

    public GameObject losePanel;
    public GameObject winPanel;
    public TMP_Text yourScore;
    public TMP_Text highScore;

    public void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
    }


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

    public void ActivateLosePanel()
    {
        losePanel.SetActive(true);
        _GM.ChangeGameState(GameState.GameOver);
    }

    public void ActivateWinPanel()
    {
        scoreText.text = yourScore.text;
        winPanel.SetActive(true);
        yourScore.text = currentScore.ToString("F3");
        highScore.text = bestScore.ToString("F3");

        if (currentScore <= bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetFloat("BestTime" + _GM.GetSceneName(), bestScore);
            highScore.text = bestScore.ToString("F3") + " !! NEW BEST !!";
        }
    }
}
