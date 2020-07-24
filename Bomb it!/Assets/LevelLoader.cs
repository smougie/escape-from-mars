using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int levelToLoad;
    [SerializeField] TMP_Text levelNumberText;
    [SerializeField] TMP_Text levelPercentageScoreText;
    [SerializeField] GameObject planet1, planet2, planet3, padlock;
    [SerializeField] GameObject levelNumberObj, levelLabelObj, planetSelectionObj, scoreBackgroundObj;

    private GameObject[] gameObjectsToDisable;
    private string levelNumberStr;
    private string levelRecord;
    private int planetScore;
    private int levelPercentageScore;
    private int levelScore;

    private int planetScoreIndex = 1;
    private int levelPercentageIndex = 2;
    private int levelScoreIndex = 3;

    void Start()
    {
        GameObject[] gameObjectsToDisable = { levelNumberObj, levelLabelObj, planetSelectionObj, scoreBackgroundObj};
        levelNumberStr = $"Level {levelToLoad}";
        levelNumberText.text = $"{levelToLoad}";
        ReadLevelRecord();
        ParseLevelRecord(levelRecord);
        if (LevelRecordEmpty())
        {
            DisableObjects(gameObjectsToDisable);
            EnablePadlock();
        }
        else
        {
            levelPercentageScoreText.text = $"{levelPercentageScore}%";
            ActivatePlanets(planetScore);
        }
    }

    private void ReadLevelRecord()
    {
        levelRecord = PlayerPrefs.GetString(levelNumberStr);
    }

    private void ParseLevelRecord(string recordToSplit)
    {
        if (levelRecord != "")
        {
            string[] recordAfterSplit = recordToSplit.Split(',');
            planetScore = int.Parse(recordAfterSplit[planetScoreIndex]);
            levelPercentageScore = int.Parse(recordAfterSplit[levelPercentageIndex]);
            levelScore = int.Parse(recordAfterSplit[levelScoreIndex]);
        }
    }

    private bool LevelRecordEmpty()
    {
        if (levelRecord == "" || levelRecord == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActivatePlanets(int planetsCount)
    {
        switch (planetsCount)
        {
            case 1:
                planet1.SetActive(true);
                planet2.SetActive(false);
                planet3.SetActive(false);
                break;
            case 2:
                planet1.SetActive(true);
                planet2.SetActive(true);
                planet3.SetActive(false);
                break;
            case 3:
                planet1.SetActive(true);
                planet2.SetActive(true);
                planet3.SetActive(true);
                break;
            default:
                planet1.SetActive(false);
                planet2.SetActive(false);
                planet3.SetActive(false);
                break;
        }
    }

    private void DisableObjects(GameObject[] gameObjects)
    {
        foreach (var item in gameObjects)
        {
            item.SetActive(false);
        }
    }

    private void EnablePadlock()
    {
        padlock.SetActive(true);
    }
}
