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
    GameOver
}
public class GameManager : Singleton<GameManager>
{
    public GameState gameState;

    public int score;
    public int scoreMultiplier = 1;

    public int blockCounter;
    public int counterCounter;

    public void ChangeGameState(GameState _gameState)
    {
        gameState = _gameState;
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        ChangeGameState(GameState.Playing);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

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
            blockCounter = 0;
            _UI.ResetBlockCounter();
        }

    }

    public void OnBreadBlocked()
    {
        AddScore(1);
        AddBlockCounter();
    }

    public void OnBreadCountered()
    {
        AddScore(3);
    }
}
