using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;


public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    [SerializeField] GameObject pauseMenu;
    [HideInInspector] public bool updateSoundValues = false;
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject optionsMenu;
    private bool optionsWindow = false;

    void Update()
    {
        SwitchPauseState();
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        UnfreezeGame();
        gameIsPaused = false;
        updateSoundValues = true;
    }

    private void Pause()
    {
        pauseMenu.SetActive(true);
        FreezeGame();
        gameIsPaused = true;
    }

    public void OptionWindowIsActive()
    {
        optionsWindow = true;
    }

    public void DisableOptionsWindow()
    {
        optionsWindow = false;
        optionsMenu.SetActive(false);
    }

    public void DisableMenuWindow()
    {
        mainMenu.SetActive(false);
    }

    public void EnableOptionsWindow()
    {
        optionsMenu.SetActive(true);
    }

    public void EnableMenuWindow()
    {
        mainMenu.SetActive(true);
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
            if (!optionsWindow)
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
            else
            {
                DisableOptionsWindow();
                EnableMenuWindow();
            }

        }
    }

    public void LoadMainMenu()
    {
        Resume();
        DestroyLeftObjects();
        SceneManager.LoadScene("Main Menu");
    }

    private void DestroyLeftObjects()
    {
        Destroy(FindObjectOfType<GameManager>().gameObject);
        Destroy(FindObjectOfType<Canvas>().gameObject);
        Destroy(FindObjectOfType<EventSystem>().gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
