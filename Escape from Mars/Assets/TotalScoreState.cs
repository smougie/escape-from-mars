using UnityEngine;

public class TotalScoreState : MonoBehaviour
{
    [SerializeField] GameObject totalScoreObject;
    [SerializeField] GameObject submitScorePromptObject;

    void Start()
    {
        //EnableTotalScore();     
        //PlayerPrefs.SetInt("GameFinished", 0);
    }

    private void OnEnable()
    {
        EnableTotalScore();
    }

    private bool ReadGameFinished()
    {
        bool gameFinished = false;
        int gameFinishedValue = PlayerPrefs.GetInt("GameFinished");

        if (gameFinishedValue == 0)
        {
            gameFinished = false;
        }
        else if (gameFinishedValue == 1)
        {
            gameFinished = true;
        }
        else
        {
            Debug.LogError("ReadGameFinished() - SaveManager.cs - Value error while reading GameFinishedValue");
        }
        return gameFinished;
    }

    private bool ReadShowSubmit()
    {
        bool showSubmitScorePrompt = false;
        int showSubmitScoreValue = PlayerPrefs.GetInt("ShowSubmitScorePrompt");

        if (showSubmitScoreValue == 1)
        {
            showSubmitScorePrompt = true;
        }
        else if (showSubmitScoreValue > 1 || showSubmitScoreValue == 0)
        {
            showSubmitScorePrompt = false;
        }
        else
        {
            Debug.LogError("ReadShowSubmit() - SaveManager.cs - Value error while reading ShowSubmitScorePrompt");
        }
        return showSubmitScorePrompt;
    }

    public void EnableTotalScore()
    {
        totalScoreObject.SetActive(ReadGameFinished());
        submitScorePromptObject.SetActive(ReadShowSubmit());
    }
}
