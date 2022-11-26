using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
}
