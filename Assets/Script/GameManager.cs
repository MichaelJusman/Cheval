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

    public void AddScore(int _score)
    {
        score += _score * scoreMultiplier;
        _UI.UpdateScore(score);
    }

    //Add block counter, if counter reaches a certain number, heals the player
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

    //Reset block counter once it hits the limit
    public void ResetBlockCounter()
    {
        blockCounter = 0;
        _UI.UpdateBlockCounter(blockCounter);
    }

    //Add counter counter, if counter reaches a certain number, heals the player
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

    //Reset counter counter once it hits the limit
    public void ResetCounterCounter()
    {
        counterCounter = 0;
        _UI.UpdateCounterCounter(counterCounter);
    }

    //Add score & counter when sucessfuly block
    public void OnBreadBlocked()
    {
        AddScore(1);
        AddBlockCounter();
    }

    //Add score & counter when sucessfuly counter
    public void OnBreadCountered()
    {
        AddScore(3);
        AddCounterCounter();
    }

    public string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
