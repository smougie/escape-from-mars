using UnityEngine;

public class TotalScoreState : MonoBehaviour
{
    [SerializeField] GameObject totalScoreObject;

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

    public void EnableTotalScore()
    {
        totalScoreObject.SetActive(ReadGameFinished());
    }
}
