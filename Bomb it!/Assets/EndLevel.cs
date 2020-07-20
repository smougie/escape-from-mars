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
