using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    private SaveManager saveManagerRef;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            saveManagerRef = GetComponent<SaveManager>();
            ReadFirstLevelRecord();
        }

    }

    private void ReadFirstLevelRecord()
    {
        string firstLevelRecord = saveManagerRef.GetLevelRecord(1);
        if (firstLevelRecord == "")
        {
            DisableContinueButton();
        }
    }

    public void NewGame()
    {
        saveManagerRef.LoadLevelSettings(1);
        saveManagerRef.ClearRecordBase();
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

    private void DisableContinueButton()
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
