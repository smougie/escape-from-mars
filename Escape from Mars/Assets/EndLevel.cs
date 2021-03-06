﻿using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] GameObject endLevelWindow;
    [SerializeField] TMP_Text percentageScoreText;
    [SerializeField] TMP_Text levelScoreText;
    [SerializeField] TMP_Text statusText;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject continueButtonShadow;
    [SerializeField] GameObject planet1;
    [SerializeField] GameObject planet2;
    [SerializeField] GameObject planet3;
    [SerializeField] GameObject buttonSection, restartButtonSection;

    private GameManager gameManagerRef;
    private SaveManager saveManagerRef;
    public bool endLevelWindowEnabled = false;
    private bool gameIsOver = false;

    void Start()
    {
        gameManagerRef = GameObject.Find("Game Manager").GetComponent<GameManager>();
        saveManagerRef = GetComponent<SaveManager>();
    }

    private void UpdatePlanets(float scorePercentage)
    {
        if (scorePercentage == 0)
        {
            DisplayPlanets(0);
        }
        else if (scorePercentage > 0 && scorePercentage <= 50)
        {
            DisplayPlanets(1);
        }
        else if (scorePercentage > 50 && scorePercentage <= 90)
        {
            DisplayPlanets(2);
        }
        else if (scorePercentage > 90 && scorePercentage <= 100)
        {
            DisplayPlanets(3);
        }
        else
        {
            Debug.LogError("UpdatePlanets() percentage value error");
        }
    }

    private void DisplayPlanets(int planetValue)
    {
        if (planetValue == 0)
        {
            planet1.SetActive(false);
            planet2.SetActive(false);
            planet3.SetActive(false);
            gameManagerRef.planetScore = 0;
        }
        else if (planetValue == 1)
        {
            planet1.SetActive(true);
            planet2.SetActive(false);
            planet3.SetActive(false);
            gameManagerRef.planetScore = 1;
        }
        else if (planetValue == 2)
        {
            planet1.SetActive(true);
            planet2.SetActive(false);
            planet3.SetActive(true);
            gameManagerRef.planetScore = 2;
        }
        else if (planetValue == 3)
        {
            planet1.SetActive(true);
            planet2.SetActive(true);
            planet3.SetActive(true);
            gameManagerRef.planetScore = 3;
        }
        else
        {
            Debug.LogError("DisplayPlanets() - planetValue error");
        }
    }

    private void UpdateStatusText(bool gameOver)
    {
        string updatedStatusText = "";
        if (gameOver)
        {
            updatedStatusText = "Failed!";
        }
        else
        {
            updatedStatusText = "Success!";
        }
        statusText.text = updatedStatusText;
    }

    private void UpdateScoreValues()
    {
        percentageScoreText.text = $"{gameManagerRef.levelPercentageScore}%";
        levelScoreText.text = $"{gameManagerRef.levelScore}";
    }

    private void EnableEndLevelWindow(bool gameOver)
    {
        endLevelWindowEnabled = true;
        endLevelWindow.SetActive(true);
        UpdatePlanets(gameManagerRef.levelPercentageScore);

        if (saveManagerRef.ReadRestartedValue() == 1)
        {
            buttonSection.SetActive(false);
            restartButtonSection.SetActive(true);
            if (gameOver)
            {
                gameIsOver = true;
            }
        }
        else if (gameOver)
        {
            restartButtonSection.SetActive(false);
            gameIsOver = true;
            continueButton.SetActive(false);
            continueButtonShadow.SetActive(false);
        }
        else
        {
            buttonSection.SetActive(true);
            restartButtonSection.SetActive(false);
            continueButton.SetActive(true);
            continueButtonShadow.SetActive(true);
        }
    }

    public void DisableEndLevelWindow()
    {
        gameIsOver = false;
        endLevelWindowEnabled = false;
        endLevelWindow.SetActive(false);
    }

    public void ShowEndLevelWindow(bool gameOver = false)
    {
        EnableEndLevelWindow(gameOver);
        UpdateStatusText(gameOver);
        UpdateScoreValues();
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadNextLevel()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = sceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings - 3)  // if sceneIndex is equal to last scene, load level select
        {
            saveManagerRef.SetGameFinished(1);  // set gameFinished value in PlayerPrefs to 1 == true;
            saveManagerRef.SetShowSubmitScorePrompt();  // set PlayerPrefs.SetInt("ShowSubmitScorePrompt") += 1
            gameManagerRef.SaveScores();
            saveManagerRef.LoadLevelSettings(0);
            gameManagerRef.DestroyLeftObjects();
            SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 3);
        }
        else
        {
            gameManagerRef.SaveScores();
            DisableEndLevelWindow();
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void LoadLevelSelect()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 4)  // if current scene is equal to last level scene (-2 because sceneinbuild is calc from 1, -1 is level select, and -2 is last level scene, -3 is tutorial scene, -4 is playground scene)
        {
            saveManagerRef.SetGameFinished(1);  // set gameFinished value in PlayerPrefs to 1 == true;
            saveManagerRef.SetShowSubmitScorePrompt();  // set PlayerPrefs.SetInt("ShowSubmitScorePrompt") += 1
        }
        if (!gameIsOver)
        {
            gameManagerRef.SaveScores();
        }
        saveManagerRef.LoadLevelSettings(0);
        saveManagerRef.ClearRestartRecord();
        gameManagerRef.DestroyLeftObjects();
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 3);
    }

    public void LoadMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 2 && !gameIsOver)
        {
            saveManagerRef.SetGameFinished(1);  // set gameFinished value in PlayerPrefs to 1 == true;
            saveManagerRef.SetShowSubmitScorePrompt();  // set PlayerPrefs.SetInt("ShowSubmitScorePrompt") += 1
        }
        if (!gameIsOver)
        {
            gameManagerRef.SaveScores();
        }
        saveManagerRef.LoadLevelSettings(0);
        saveManagerRef.ClearRestartRecord();
        gameManagerRef.DestroyLeftObjects();
        SceneManager.LoadScene(0);
    }
}
