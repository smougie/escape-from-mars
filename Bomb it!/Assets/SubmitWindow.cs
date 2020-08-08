using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SubmitWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField playerNameInputField;
    [SerializeField] TMP_Text inputFieldWarning;
    [SerializeField] GameObject inputFieldWarningObj;
    private SaveManager saveManagerRef;
    private enum WarningType { Empty, Exists}
    WarningType warningType;

    void Start()
    {
        saveManagerRef = GetComponent<SaveManager>();
        //PlayerPrefs.DeleteKey("Highscores");
        //saveManagerRef.SetGameFinished(0);
    }

    public bool PlayerNameEmpty()
    {
        bool playerNameEmpty = false;
        if (playerNameInputField.text.Length > 0)
        {
            playerNameEmpty = false;
        }
        else
        {
            playerNameEmpty = true;
        }
        return playerNameEmpty;
    }

    public bool PlayerNameInHighscoreRecord()
    {
        bool playerNameInHighscoreRecord = false;
        string playerName = playerNameInputField.text;
        Dictionary<string, string> highscoreRecord = saveManagerRef.GetHighscoresDict();
        foreach (var playerNameRecord in highscoreRecord.Keys)
        {
            if (playerName == playerNameRecord)
            {
                playerNameInHighscoreRecord = true;
            }
        }

        return playerNameInHighscoreRecord;
    }

    public void SubmitTotalScoreSequence()
    {
        CheckPlayerNameNotEmpty();
    }

    private void CheckPlayerNameNotEmpty()
    {
        if (PlayerNameEmpty())  // if player name input field is NOT EMPTY
        {
            DisplayPlayerNameInputWarning(warningType = WarningType.Empty);
        }
        else  // if player name input field is EMPTY
        {
            CheckHigscoreRecord();
        }
    }

    private void CheckHigscoreRecord()
    {
        if (saveManagerRef.HighscoreRecordEmpty())  // if Highscore Record is EMPTY
        {
            SubmitTotalScore();
            
        }
        else  // if Highscore Record is NOT EMPTY
        {
            CheckPlayerNameInRecord();
        }
    }

    private void CheckPlayerNameInRecord()
    {
        if (PlayerNameInHighscoreRecord())
        {
            DisplayPlayerNameInputWarning(warningType = WarningType.Exists);
        }
        else
        {
            SubmitTotalScore();
        }
    }

    private void SubmitTotalScore()
    {
        // TODO inser player name and score into highscores record base
        saveManagerRef.SetGameFinished(0);
        saveManagerRef.InsertHighscoreToRecordBase(playerNameInputField.text);
        saveManagerRef.ClearRecordBase();
        SceneManager.LoadScene("Main Menu");  // load main menu with opened Highscores Window
                                              // TODO remove all scores (level X scores, total scores)
    }

    private void DisplayPlayerNameInputWarning(WarningType warningType)
    {
        string warning = "";
        switch (warningType)
        {
            case WarningType.Empty:
                warning = "Please enter player name.";
                break;
            case WarningType.Exists:
                warning = "Player name already exists.";
                break;
            default:
                break;
        }
        inputFieldWarning.text = warning;
        inputFieldWarningObj.SetActive(true);
    }
}
