using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject newGameWarning;
    private SaveManager saveManagerRef;
    private string firstLevelRecord;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            saveManagerRef = GetComponent<SaveManager>();
            ReadFirstLevelRecord();
            VerifyContinueState();
        }

    }

    private void VerifyContinueState()
    {
        if (firstLevelRecord == "")
        {
            DisableContinueButton();
        }
    }

    private void ReadFirstLevelRecord()
    {
        firstLevelRecord = saveManagerRef.GetLevelRecord(1);
    }

    public void NewGame()
    {
        if (firstLevelRecord == "")  // if there is no level records start a new game
        {
            StartNewGame();
        }
        else  // if there is any record in record base, show new game warning
        {
            DisableMainMenu();
            EnableNewGameWaning();
        }

    }
    public void StartNewGame()
    {
        saveManagerRef.LoadLevelSettings(1);
        saveManagerRef.SetGameFinished(0);
        saveManagerRef.ClearShowSubmitScorePrompt();
        saveManagerRef.ClearRecordBase();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 3);  // load level selection level with index -2
    }

    public void EnableNewGameWaning()
    {
        newGameWarning.SetActive(true);
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
