﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject life1;
    [SerializeField] GameObject life2;
    [SerializeField] GameObject life3;

    public static int maxLife = 3;
    public static int currentLife;
    private bool alive = true;
    public static int maxLevelCollectibles;
    public static int currentCollectiblesValue = 0;
    public static int totalCollectibles = 0;
    private float lifeScore = 0;
    private float collectiblesScore = 0;
    private float levelScore = 0;
    private float totalScore = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentLife = maxLife;
        maxLevelCollectibles = GameObject.Find("Collectibles").transform.childCount;
    }

    void Update()
    {
        //print($"Life: {currentLife}/{maxLife}\nTotal C: {totalCollectibles}");
        //print($"Collectibles: {currentCollectiblesValue}/{maxLevelCollectibles}");
        if (alive)
        {
            CheckLifeStatus();
        }
        UpdateLifeBar();
    }
    
    private void CheckLifeStatus()
    {
        if (currentLife <= 0)
        {
            // TODO game over, think about moving this check method to StartDeathSequence() to check it just once
            print("GAME OVER");
        }
    }

    private void UpdateLifeBar()
    {
        switch (currentLife)
        {
            case 1:
                life1.SetActive(true);
                life2.SetActive(false);
                life3.SetActive(false);
                break;
            case 2:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(false);
                break;
            case 3:
                life1.SetActive(true);
                life2.SetActive(true);
                life3.SetActive(true);
                break;
            default:
                life1.SetActive(false);
                life2.SetActive(false);
                life3.SetActive(false);
                break;
        }
    }

    public bool CanCollectLife()
    {
        return currentLife < maxLife;
    }

    public void DecreaseLife()
    {
        currentLife -= 1;
    }

    public void IncreaseLife()
    {
        if (CanCollectLife())
        {
            currentLife += 1;
        }
    }

    public void IncreaseCollectiblesCount()
    {
        currentCollectiblesValue += 1;
        totalCollectibles += 1;
    }

    public void ResetLevelCollectiblesCount()
    {
        currentCollectiblesValue = 0;
    }

    public void CalculateLevelScore()
    {
        collectiblesScore = (float)currentCollectiblesValue / (float)maxLevelCollectibles;
        lifeScore = (float)currentLife / (float)maxLife;
        levelScore = Mathf.Round(((collectiblesScore + lifeScore) / 2f) * 100f);

        UpdateTotalScore();
        ResetScoreValues();
    }

    private void UpdateTotalScore()
    {
        totalScore += levelScore;
    }

    private void ResetScoreValues()
    {
        collectiblesScore = 0f;
        lifeScore = 0f;
        levelScore = 0f;
    }
}
