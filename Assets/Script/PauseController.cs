using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : GameBehaviour
{
    public GameObject pausePanel;
    bool isPaused = false;


    private void Start()
    {
        pausePanel.SetActive(false);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglPause();
        }
    }
    public void TogglPause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            _EM.audioSource.Pause();
        }
        else
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
            _EM.audioSource.UnPause();
        }
    }
}
