using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] int levelToLoad;
    [SerializeField] int tempPlanets;
    [SerializeField] int tempPercentage;
    [SerializeField] TMP_Text levelNumberText;
    [SerializeField] TMP_Text levelPercentageScoreText;
    [SerializeField] GameObject planet1, planet2, planet3;

    void Start()
    {
        levelNumberText.text = $"{levelToLoad}";
        levelPercentageScoreText.text = $"{tempPercentage}%";  // TODO add % score from PP
        ActivatePlanets(tempPlanets);
    }

    void Update()
    {
        
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
}
