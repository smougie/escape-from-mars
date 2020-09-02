using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject life1;
    [SerializeField] GameObject life2;
    [SerializeField] GameObject life3;
    [SerializeField] TMP_Text textMeshProText;
    [SerializeField] GameObject canvasObject;
    [SerializeField] GameObject eventSystemObject;
    private Image life1Image;
    private Image life2Image;
    private Image life3Image;
    private SaveManager saveManagerRef;

    float imageScaleDuration = .25f;

    private float[] levelScoreImportance = new float[] { 100f, 150f, 200f, 250f, 300f, 350f, 400f, 500f};  // those values stores int which is added to level score value after completing level, index == level

    public static int maxLife = 3;
    public static int currentLife;
    public static int maxLevelCollectibles;
    public static int currentCollectiblesValue = 0;
    private float lifeScore = 0f;
    private float collectiblesScore = 0f;
    public float levelScore = 0f;
    public float totalScore = 0f;
    public int planetScore = 0;
    public float levelPercentageScore = 0f;
    private string collectiblesStatusText;
    private GameObject collectiblesBarUI;
    private bool collectiblesAvailable = true;

    void Start()
    {
        saveManagerRef = GetComponent<SaveManager>();
        if (CheckForLevelSettings())
        {
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(canvasObject);
            DontDestroyOnLoad(eventSystemObject);
            life1Image = life1.GetComponent<Image>();
            life2Image = life2.GetComponent<Image>();
            life3Image = life3.GetComponent<Image>();
            currentLife = maxLife;
            collectiblesBarUI = GameObject.Find("Alien Bar");
            if (GameObject.Find("Collectibles").transform.childCount != 0)
            {
                maxLevelCollectibles = GameObject.Find("Collectibles").transform.childCount;
                textMeshProText.text = collectiblesStatusText;
                UpdateCollectiblesStatus();
            }
            else
            {
                collectiblesAvailable = false;
                DisableCollectiblesUI();
            }

            saveManagerRef.LoadLevelSettings(0);
        }
        else
        {
            canvasObject.SetActive(false);
            eventSystemObject.SetActive(false);
            gameObject.SetActive(false);
            ResetCollectibles();
            ResetLifes();
        }
    }

    void Update()
    {
        if (collectiblesAvailable)
        {
            UpdateCollectiblesStatus();
        }
        if (Alive())
        {
            UpdateLifeBar();
        }
    }

    private bool CheckForLevelSettings()
    {
        if (saveManagerRef.ReadLevelSettings() == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
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
    }
    
    private void ReloadCurrentLevel()
    {
        if (GetActiveLevelIndex() != 1)
        {
            SceneManager.LoadScene(GetActiveLevelIndex());
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    public void RepeatLevel()
    {
        ResetScoresValues();
        ResetLevelValues();
        ReloadCurrentLevel();
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

    public bool CurrentlyMaxLife()
    {
        if (currentLife == maxLife)
        {
            return true;
        }
        else
        {
            return false;
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
    }

    public void IncreaseLife()
    {
        if (CanCollectLife())
        {
            currentLife += 1;
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
    }

    public void ResetLevelCollectiblesCount()
    {
        currentCollectiblesValue = 0;
    }

    public void CalculateLevelScore()
    {
        collectiblesScore = (float)currentCollectiblesValue / (float)maxLevelCollectibles;  // calculate collectibles percentage value (1/2 collectibles == .5f score)
        lifeScore = (float)currentLife / (float)maxLife;  // calculate life percentage value (1/3 collectibles == .33f score)
        levelPercentageScore = Mathf.Round(((collectiblesScore + lifeScore) / 2f) * 100f);  // calculate level percentage score (average from collectibles and life score)
        levelScore = levelPercentageScore + levelScoreImportance[GetActiveLevelIndex() - 1];
    }

    public void SaveScores()
    {
        saveManagerRef.SaveLevelRecord(GetActiveLevelIndex(), planetScore, levelPercentageScore, levelScore);  // send record to PP base
        UpdateTotalScore();  // update total score
        saveManagerRef.UnlockNextLevel(GetActiveLevelIndex());  // unlock next level for level select scene
        ResetScoresValues();  // reset all scores values
    }

    private void UpdateTotalScore()
    {
        int currentTotalScore = saveManagerRef.GetTotalScore();
        if (LevelRestarted())
        {
            int oldLevelScore = saveManagerRef.GetOldLevelScore();
            currentTotalScore = (currentTotalScore - oldLevelScore) + (int)Mathf.Round(levelScore);
        }
        else
        {
            currentTotalScore = currentTotalScore + (int)Mathf.Round(levelScore);
        }
        saveManagerRef.SaveTotalScore(currentTotalScore);
        
    }

    private bool LevelRestarted()
    {
        if (saveManagerRef.ReadRestartedValue() == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ResetScoresValues()
    {
        currentCollectiblesValue = 0;
        collectiblesScore = 0f;
        lifeScore = 0f;
        levelPercentageScore = 0f;
        levelScore = 0f;
        planetScore = 0;
    }

    private int GetActiveLevelIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void DestroyLeftObjects()
    {
        Destroy(FindObjectOfType<GameManager>().gameObject);
        Destroy(FindObjectOfType<Canvas>().gameObject);
        Destroy(FindObjectOfType<EventSystem>().gameObject);
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
