using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelDetails : MonoBehaviour
{
    [SerializeField] TMP_Text levelLabelTxt, percentageScoreTxt, levelScoreTxt;
    PlanetControl planetControlRef;

    public void UpdateLevelDetails(int levelNumber, int planetScore,int percentageScore, int levelScore)
    {
        planetControlRef = GetComponent<PlanetControl>();
        planetControlRef.ActivatePlanets(planetScore);
        levelLabelTxt.text = $"Level {levelNumber}";
        percentageScoreTxt.text = $"{percentageScore}%";
        levelScoreTxt.text = $"{levelScore}";
    }
}
