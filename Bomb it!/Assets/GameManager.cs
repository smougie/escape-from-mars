using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static int maxLife = 3;
    static int currentLife;
    private bool alive = true;
    static int maxLevelCollectibles;
    static int currentCollectiblesValue = 0;
    static int totalCollectibles = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        currentLife = maxLife;
        maxLevelCollectibles = GameObject.Find("Collectibles").transform.childCount;
    }

    void Update()
    {
        if (alive)
        {
            CheckLifeStatus();
        }
    }
    
    private void CheckLifeStatus()
    {
        if (currentLife <= 0)
        {
            // TODO game over, think about moving this check method to StartDeathSequence() to check it just once
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
        currentLife += 1;
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
}
