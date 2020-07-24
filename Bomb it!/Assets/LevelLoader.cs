using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int levelToLoad;
    [SerializeField] TMP_Text levelNumberText;
    [SerializeField] TMP_Text levelPercentageScoreText;
    [SerializeField] GameObject levelDetailsObj, planet1, planet2, planet3, padlock, continueButtonObj;
    [SerializeField] GameObject levelNumberObj, levelLabelObj, planetSelectionObj, scoreBackgroundObj;

    private Button levelBoxButton;

    private GameObject[] gameObjectsToDisable;
    private GameObject levelSectionObj;
    private string levelNumberStr;
    private string levelUnlockedStr;
    private string levelRecord;
    private int planetScore;
    private int levelPercentageScore;
    private int levelScore;

    private int planetScoreIndex = 1;
    private int levelPercentageIndex = 2;
    private int levelScoreIndex = 3;

    void Start()
    {
        levelSectionObj = GameObject.Find("Levels Section");
        levelBoxButton = GetComponent<Button>();
        GameObject[] gameObjectsToDisable = { levelNumberObj, levelLabelObj, planetSelectionObj, scoreBackgroundObj};
        levelNumberStr = $"Level {levelToLoad}";
        levelUnlockedStr = $"{levelToLoad},0,0,0";
        levelNumberText.text = $"{levelToLoad}";
        ReadLevelRecord();
        ParseLevelRecord(levelRecord);
        if (LevelRecordEmpty())
        {
            DisableObjects(gameObjectsToDisable);
            ButtonNotInteractable();
            EnablePadlock();
        }
        else if (levelRecord == levelUnlockedStr)
        {
            DisableObjects(new GameObject[] { scoreBackgroundObj, planet1, planet2, planet3});
            ButtonNotInteractable();
            EnableContinue();
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

    private void EnableContinue()
    {
        continueButtonObj.SetActive(true);
    }

    private void EnablePadlock()
    {
        padlock.SetActive(true);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    private void ButtonNotInteractable()
    {
        levelBoxButton.interactable = false;
    }

    public void RestartLevel()
    {

    }

    public void EnableLevelSection(bool enabled)
    {
        if (enabled)
        {
            levelSectionObj.SetActive(true);
        }
        else
        {
            levelSectionObj.SetActive(false);
        }
    }

    public void EnableLevelDetails(bool enabled)
    {
        GameObject[] objectsToDisable = { GameObject.Find("Frame"), GameObject.Find("Level Select Title")};
        if (enabled)
        {
            levelDetailsObj.SetActive(true);
            DisableObjects(objectsToDisable);
        }
        else
        {
            levelDetailsObj.SetActive(false);
        }
    }
}
