using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using static GameManager;

public enum GameState
{
    Title,
    Playing,
    Paused,
    GameOver,
    Win
}
public class GameManager : Singleton<GameManager>
{
    public GameState gameState;

    public int score;
    public int scoreMultiplier = 1;

    public int blockCounter;
    public int counterCounter;


    public void Start()
    {
        blockCounter = 0;
        _UI.UpdateBlockCounter(blockCounter);
        counterCounter = 0;
        _UI.UpdateCounterCounter(counterCounter);
        ChangeGameState(GameState.Playing);
    }

    public void ChangeGameState(GameState _gameState)
    {
        gameState = _gameState;
    }


    //public void StartGame()
    //{
    //    SceneManager.LoadScene("MainGame");
    //    ChangeGameState(GameState.Playing);
    //}

    //public void LoadTitle()
    //{
    //    SceneManager.LoadScene("Title");
    //    ChangeGameState(GameState.Title);
    //}

    //public void QuitGame()
    //{
    //    Application.Quit();
    //}

    public void AddScore(int _score)
    {
        score += _score * scoreMultiplier;
        _UI.UpdateScore(score);
    }

    public void AddBlockCounter()
    {
        blockCounter ++;
        _UI.UpdateBlockCounter(blockCounter);
        if (blockCounter == 10)
        {
            _PC.Heal(5);
            ResetBlockCounter();
        }
    }

    public void ResetBlockCounter()
    {
        blockCounter = 0;
        _UI.UpdateBlockCounter(blockCounter);
    }

    public void AddCounterCounter()
    {
        counterCounter++;
        _UI.UpdateCounterCounter(counterCounter);
        if (counterCounter == 5)
        {
            _PC.Heal(5);
            ResetCounterCounter();
        }
    }

    public void ResetCounterCounter()
    {
        counterCounter = 0;
        _UI.UpdateCounterCounter(counterCounter);
    }

    public void OnBreadBlocked()
    {
        AddScore(1);
        AddBlockCounter();
    }

    public void OnBreadCountered()
    {
        AddScore(3);
        AddCounterCounter();
    }

    //IEnumerator Victory()
    //{
    //    isPlaying = true;
    //    new WaitForSeconds(138);
    //    isPlaying = false;
    //    ChangeGameState(GameState.Win);
    //    yield return null;
    //}

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
