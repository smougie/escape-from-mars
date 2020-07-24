using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;

    void Start()
    {
        ReadFirstLevelRecord();
    }

    private void ReadFirstLevelRecord()
    {
        string firstLevelRecord = PlayerPrefs.GetString("Level 1");
        if (firstLevelRecord == "")
        {
            DisableContinue();
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);  // load last scene in build (level selection)
    }

    public void DisableMainMenu()
    {
        gameObject.SetActive(false);
    }

    public void EnableMainMenu()
    {
        gameObject.SetActive(true);
    }

    private void DisableContinue()
    {
        continueButton.SetActive(false);
    }

    private void EnableContinue()
    {
        continueButton.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
