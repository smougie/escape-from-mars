﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject life1;
    [SerializeField] GameObject life2;
    [SerializeField] GameObject life3;
    [SerializeField] TMP_Text textMeshProText;
    [SerializeField] bool collectibles = false;
    [SerializeField] GameObject canvasObject;
    [SerializeField] GameObject eventSystemObject;
    private Image life1Image;
    private Image life2Image;
    private Image life3Image;

    float imageScaleDuration = .25f;

    private float[] levelScoreImportance = new float[] { 100f, 200f, 300f, 400f, 500f, 600f, 700f};  // those values stores int which is added to level score value after completing level, index == level

    public static int maxLife = 3;
    public static int currentLife;
    public static int maxLevelCollectibles;
    public static int currentCollectiblesValue = 0;
    public static int totalCollectibles = 0;
    private float lifeScore = 0f;
    private float collectiblesScore = 0f;
    public float levelScore = 0f;
    public float totalScore = 0f;
    public float levelPercentageScore = 0f;
    private string collectiblesStatusText;
    private GameObject collectiblesBarUI;
    public bool newGame = false;
    private int currentLevelIndex;

    void Start()
    {
        if (newGame)
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(canvasObject);
            DontDestroyOnLoad(eventSystemObject);
            currentLife = maxLife;
            life1Image = life1.GetComponent<Image>();
            life2Image = life2.GetComponent<Image>();
            life3Image = life3.GetComponent<Image>();
            collectiblesBarUI = GameObject.Find("Alien Bar");
            if (collectibles)
            {
                maxLevelCollectibles = GameObject.Find("Collectibles").transform.childCount;
                textMeshProText.text = collectiblesStatusText;
                UpdateCollectiblesStatus();
            }
            else
            {
                DisableCollectiblesUI();
            }
        }
        else
        {
            canvasObject.SetActive(false);
            eventSystemObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void SetNewGame()
    {
        newGame = true;
    }

    private void EnableCollectiblesUI()
    {
        collectiblesBarUI.SetActive(true);
    }

    private void DisableCollectiblesUI()
    {
        collectiblesBarUI.SetActive(false);
    }

    public void UpdateCollectiblesStatus()
    {
        collectiblesStatusText = $"{currentCollectiblesValue}/{maxLevelCollectibles}";
        textMeshProText.text = collectiblesStatusText;
    }

    public void ResetCollectibles()
    {
        currentCollectiblesValue = 0;
        maxLevelCollectibles = GameObject.Find("Collectibles").transform.childCount;
    }

    public void ResetLifes()
    {
        currentLife = maxLife;
    }

    public void ResetLevelValues()
    {
        ResetCollectibles();
        ResetLifes();
        UpdateCollectiblesStatus();
        UpdateLifeBar();
    }

    public bool Alive()
    {
        if (currentLife <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void FadeOutImage(Image image, bool fadeOut)
    {
        float alphaValue = .3f;
        if (fadeOut)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, alphaValue);
        }
        else
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        }
    }
    
    public void UpdateLifeBar()
    {
        switch (currentLife)
        {
            case 1:
                FadeOutImage(life1Image, false);
                FadeOutImage(life2Image, true);
                FadeOutImage(life3Image, true);
                break;
            case 2:
                FadeOutImage(life1Image, false);
                FadeOutImage(life2Image, false);
                FadeOutImage(life3Image, true);
                break;
            case 3:
                FadeOutImage(life1Image, false);
                FadeOutImage(life2Image, false);
                FadeOutImage(life3Image, false);
                break;
            default:
                FadeOutImage(life1Image, true);
                FadeOutImage(life2Image, true);
                FadeOutImage(life3Image, true);
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
        UpdateLifeBar();
    }

    public void IncreaseLife()
    {
        if (CanCollectLife())
        {
            currentLife += 1;
            UpdateLifeBar();
            ScaleImage();
        }
    }

    private void ScaleImage()
    {
        switch (currentLife)
        {
            case 1:
                StartCoroutine(ScaleUpImage(life1Image, imageScaleDuration));
                break;
            case 2:
                StartCoroutine(ScaleUpImage(life2Image, imageScaleDuration));
                break;
            case 3:
                StartCoroutine(ScaleUpImage(life3Image, imageScaleDuration));
                break;
            default:
                break;
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
        currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        collectiblesScore = (float)currentCollectiblesValue / (float)maxLevelCollectibles;  // calculate collectibles percentage value (1/2 collectibles == .5f score)
        lifeScore = (float)currentLife / (float)maxLife;  // calculate life percentage value (1/3 collectibles == .33f score)
        levelPercentageScore = Mathf.Round(((collectiblesScore + lifeScore) / 2f) * 100f);  // calculate level percentage score (average from collectibles and life score)
        levelScore = levelPercentageScore + levelScoreImportance[currentLevelIndex - 1];
    }

    public void SaveScores()
    {
        UpdateTotalScore();
        ResetScoresValues();
    }

    private void UpdateTotalScore()
    {
        totalScore += levelScore;
    }

    private void ResetScoresValues()
    {
        collectiblesScore = 0f;
        lifeScore = 0f;
        levelPercentageScore = 0f;
        levelScore = 0f;
    }

    IEnumerator ScaleUpImage(Image image, float duration)
    {

        Vector2 startSize = image.rectTransform.sizeDelta;
        Vector2 endSize = startSize * 1.8f;
        var imageXvalue = startSize.x;
        var imageYvalue = startSize.y;

        for (float time = 0f; time < duration / 2; time += Time.deltaTime)
        {
            float normalizedTime = time / duration;
            imageXvalue = Mathf.Lerp(startSize.x, endSize.x, normalizedTime);
            imageYvalue = Mathf.Lerp(startSize.y, endSize.y, normalizedTime);
            image.rectTransform.sizeDelta = new Vector2(imageXvalue, imageYvalue);
            yield return null;
        }
        for (float time = duration / 2; time < duration; time += Time.deltaTime)
        {
            float normalizedTime = time / duration;
            imageXvalue = Mathf.Lerp(endSize.x, startSize.x, normalizedTime);
            imageYvalue = Mathf.Lerp(endSize.y, startSize.y, normalizedTime);
            image.rectTransform.sizeDelta = new Vector2(imageXvalue, imageYvalue);
            yield return null;
        }
        image.rectTransform.sizeDelta = startSize;
    }
}
