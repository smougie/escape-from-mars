using UnityEngine;
using TMPro;

public class EndLevel : MonoBehaviour
{
    [SerializeField] GameObject endLevelWindow;
    [SerializeField] TMP_Text percentageScoreText;
    [SerializeField] TMP_Text levelScoreText;

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

    private void UpdateScoreValues()
    {
        percentageScoreText.text = $"{gameManagerRef.levelPercentageScore}%";
        levelScoreText.text = $"{gameManagerRef.levelScore}";
    }

    private void EnableEndLevelWindow()
    {
        endLevelWindow.SetActive(true);
    }

    private void DisableEndLevelWindow()
    {
        endLevelWindow.SetActive(false);
    }

    public void ShowEndLevelWindow()
    {
        EnableEndLevelWindow();
        UpdateScoreValues();
    }

    
}
