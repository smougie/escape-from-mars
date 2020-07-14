using UnityEngine.SceneManagement;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    [SerializeField] GameObject pauseMenu;

    void Start()
    {
    }

    void Update()
    {
        SwitchPauseState();
    }

    public void DUPA()
    {
        print("DUPA W CHUJ");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        UnfreezeGame();
        gameIsPaused = false;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        FreezeGame();
        gameIsPaused = true;
    }

    private void FreezeGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    private void UnfreezeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }

    private void SwitchPauseState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        print("Quit");
    }
}
