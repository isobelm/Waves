using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;

    private bool isPaused = false;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }

    public bool GetIsGamePaused()
    {
        return isPaused;
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            } else
            {
                UnpauseGame();
            }
        }
    }

    public void PlayerDied()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayerWon()
    {
        SceneManager.LoadScene(3);
    }
}
