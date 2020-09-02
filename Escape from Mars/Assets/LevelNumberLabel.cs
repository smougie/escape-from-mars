using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelNumberLabel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelLabelTxt;

    void Start()
    {
        UpdateLevelLabelTxt();
    }

    private void OnEnable()
    {
        UpdateLevelLabelTxt();
    }

    private void OnDisable()
    {
        ClearLevelLabelTxt();
    }

    private void UpdateLevelLabelTxt()
    {
        levelLabelTxt.text = $"Level {SceneManager.GetActiveScene().buildIndex}";
    }

    private void ClearLevelLabelTxt()
    {
        levelLabelTxt.text = "";
    }
}
