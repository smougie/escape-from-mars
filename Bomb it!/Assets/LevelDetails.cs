using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelDetails : MonoBehaviour
{
    [SerializeField] TMP_Text levelLabelTxt, percentageScoreTxt, levelScoreTxt;
    PlanetControl planetControlRef;
    SaveManager saveManagerRef;
    int currentScore;
    int levelToRestart;

    void Start()
    {
        saveManagerRef = GetComponent<SaveManager>();    
    }

    public void UpdateLevelDetails(int levelNumber, int planetScore,int percentageScore, int levelScore)
    {
        planetControlRef = GetComponent<PlanetControl>();
        planetControlRef.ActivatePlanets(planetScore);
        levelLabelTxt.text = $"Level {levelNumber}";
        percentageScoreTxt.text = $"{percentageScore}%";
        levelScoreTxt.text = $"{levelScore}";
        currentScore = levelScore;
        levelToRestart = levelNumber;
    }

    public void RestartLevel()
    {
        saveManagerRef.LoadLevelSettings(1);
        saveManagerRef.SetRestartRecord(currentScore);
        SceneManager.LoadScene(levelToRestart);
    }
}
