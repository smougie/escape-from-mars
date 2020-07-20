using UnityEngine;
using TMPro;

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

    private GameManager gameManagerRef;

    private void Awake()
    {
        gameManagerRef = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Start()
    {
        gameManagerRef = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        
    }

    private void UpdatePlanets(float scorePercentage)
    {
        if (scorePercentage == 0)
        {
            DisplayPlanets(0);
        }
        else if (scorePercentage > 0 && scorePercentage <= 33)
        {
            DisplayPlanets(1);
        }
        else if (scorePercentage > 33 && scorePercentage <= 66)
        {
            DisplayPlanets(2);
        }
        else if (scorePercentage > 66 && scorePercentage <= 100)
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
        }
        else if (planetValue == 1)
        {
            planet1.SetActive(true);
            planet2.SetActive(false);
            planet3.SetActive(false);
        }
        else if (planetValue == 2)
        {
            planet1.SetActive(true);
            planet2.SetActive(true);
            planet3.SetActive(false);
        }
        else if (planetValue == 3)
        {
            planet1.SetActive(true);
            planet2.SetActive(true);
            planet3.SetActive(true);
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
        endLevelWindow.SetActive(true);
        UpdatePlanets(gameManagerRef.levelPercentageScore);
        if (gameOver)
        {
            continueButton.SetActive(false);
            continueButtonShadow.SetActive(false);
        }
    }

    public void DisableEndLevelWindow()
    {
        endLevelWindow.SetActive(false);
    }

    public void ShowEndLevelWindow(bool gameOver = false)
    {
        EnableEndLevelWindow(gameOver);
        UpdateStatusText(gameOver);
        UpdateScoreValues();
    }

    
}
