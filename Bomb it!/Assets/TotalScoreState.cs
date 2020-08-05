using System.Collections;
using UnityEngine;

public class TotalScoreState : MonoBehaviour
{
    [SerializeField] GameObject totalScoreObject;

    void Start()
    {
        totalScoreObject.SetActive(ReadGameFinished());        
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

    private void UpdateTotalScoreValues()
    {

    }

    public void EnableTotalScore()
    {
        totalScoreObject.SetActive(ReadGameFinished());
    }
}
