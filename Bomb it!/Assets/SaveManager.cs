using System;
using System.Collections.Generic;
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

    public void SetGameFinished(int gameFinished)
    {
        PlayerPrefs.SetInt("GameFinished", gameFinished);
    }

    public bool HighscoreRecordEmpty()
    {
        bool empty = false;
        string highscoreRecord = GetHigscoreBase();
        if (highscoreRecord == "")
        {
            empty = true;
        }
        else
        {
            empty = false;
        }
        return empty;
    }

    public string GetHigscoreBase()
    {
        return PlayerPrefs.GetString("Highscores");
    }

    public int GetTotalScore()
    {
        return PlayerPrefs.GetInt("TotalScore");
    }

    public void InsertHighscoreToRecordBase(string playerName)
    {
        string newHighscoreRecord = GetHigscoreBase();
        if (newHighscoreRecord == "")
        {
            newHighscoreRecord = $"{playerName},{GetTotalScore()};";
        }
        else
        {
            newHighscoreRecord += $"{playerName},{GetTotalScore()};";
        }
        PlayerPrefs.SetString("Highscores", newHighscoreRecord);
    }

    public Dictionary<string, string> GetHighscoresDict()
    {
        Dictionary<string, string> highscoreDict = new Dictionary<string, string>();
        string highscoreRecord = GetHigscoreBase();  // "playerName,playerScore;playerName,playerScore;playerName,playerScore"
        print("Highscore record: " + highscoreRecord);
        string[] highscoreRecordSplitted = highscoreRecord.Split(';');  // ["playerName,playerScore", "playerName,playerScore"]
        print("length:" + highscoreRecordSplitted.Length);
        for (int i = 0; i < highscoreRecordSplitted.Length; i++)
        {
            print($"iteration number - {i}: " + highscoreRecordSplitted[i]);
        }



        //foreach (var record in highscoreRecordSplitted)
        //{
        //    string[] playerNameAndScore;
        //    playerNameAndScore = record.Split(',');  // ["playerName", "playerScore"]
        //    highscoreDict.Add(playerNameAndScore[0], playerNameAndScore[1]);  // playerNameAndScore[0] == "playerName" - key || playerNameAndScore[1] == "playerScore" - value
        //}

        return highscoreDict;
    }
}
