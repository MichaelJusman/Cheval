using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
        _GM.ChangeGameState(GameState.Playing);
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("Title");
        _GM.ChangeGameState(GameState.Title);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
