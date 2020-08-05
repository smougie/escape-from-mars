using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    private int technicalScenesCount = 2;  // main menu, level selection
    private int levelScenesCount;

    void Start()
    {
        levelScenesCount = SceneManager.sceneCountInBuildSettings - technicalScenesCount;    
    }

    public int GetTotalScore()
    {
        return PlayerPrefs.GetInt("TotalScore");
    }

    public void LoadLevelSettings(int loadSettings)
    {
        PlayerPrefs.SetInt("LoadLevelSettings", loadSettings);
    }

    public int ReadLevelSettings()
    {
        return PlayerPrefs.GetInt("LoadLevelSettings");
    }

    public string GetLevelRecord(int levelNumber)
    {
        return PlayerPrefs.GetString($"Level {levelNumber}");
    }

    public void UnlockNextLevel(int currentLevel)
    {
        int nextLevelIndex = currentLevel + 1;
        if (GetLevelRecord(nextLevelIndex) == "")
        {
            PlayerPrefs.SetString($"Level {nextLevelIndex}", $"{nextLevelIndex},{0},{0},{0}");
        }
    }

    public int GetOldLevelScore()
    {
        return PlayerPrefs.GetInt("CurrentScore");
    }

    public void SaveLevelRecord(int levelNumber, int planetScore, float levelPercentageScore, float levelScore)
    {
        PlayerPrefs.SetString($"Level {levelNumber}", $"{levelNumber},{planetScore},{levelPercentageScore},{levelScore}");
    }

    public void SaveTotalScore(int newTotalScore)
    {
        PlayerPrefs.SetInt("TotalScore", newTotalScore);
    }

    public void SetRestartRecord(int currentScore)
    {
        PlayerPrefs.SetInt("Restarted", 1);
        PlayerPrefs.SetInt("CurrentScore", currentScore);
    }

    public void ClearRestartRecord()
    {
        PlayerPrefs.SetInt("Restarted", 0);
        PlayerPrefs.DeleteKey("CurrentScore");
    }

    public int ReadRestartedValue()
    {
        return PlayerPrefs.GetInt("Restarted");
    }

    public void ClearRecordBase()
    {
        int levelScenesCount = SceneManager.sceneCountInBuildSettings - technicalScenesCount;
        for (int i = 1; i <= levelScenesCount; i++)
        {
            PlayerPrefs.DeleteKey($"Level {i}");
        }
        PlayerPrefs.SetInt("TotalScore", 0);
    }

    public void CreateRecordsBase()
    {
        for (int i = 1; i <= levelScenesCount; i++)
        {
            PlayerPrefs.SetString($"Level {i}", $"{i},0,0,0");
        }
    }

    public void SaveHighscore()
    {

    }

    public void SetGameFinished(int gameFinished)
    {
        PlayerPrefs.SetInt("GameFinished", gameFinished);
    }

    //public bool ReadGameFinished()
    //{
    //    bool gameFinished = false;
    //    int gameFinishedValue = PlayerPrefs.GetInt("GameFinished");

    //    if (gameFinishedValue == 0)
    //    {
    //        gameFinished = false;
    //    }
    //    else if (gameFinishedValue == 1)
    //    {
    //        gameFinished = true;
    //    }
    //    else
    //    {
    //        Debug.LogError("ReadGameFinished() - SaveManager.cs - Value error while reading GameFinishedValue");
    //    }
    //    return gameFinished;
    //}
}
