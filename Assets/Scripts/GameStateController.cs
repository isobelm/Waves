using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu = null;

    private bool isPaused = false;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        AudioManager.instance.PlayMusic(MusicSound.MusicName.LEVEL_ONE_MUSIC);
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
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySound(SoundEffectSound.SoundName.DEAD);
    }

    public void PlayerWon()
    {
        SceneManager.LoadScene(3);
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySound(SoundEffectSound.SoundName.WIN);
    }
}
