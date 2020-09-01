using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RocketCollectible : MonoBehaviour
{
    private GameManager gameManagerRef;
    [SerializeField] GameObject lifePromptObj;
    private TextMeshProUGUI lifePromptText; 

    void Start()
    {
        gameManagerRef = GameObject.Find("Game Manager").gameObject.GetComponent<GameManager>();
        lifePromptText = lifePromptObj.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            if (gameManagerRef.CurrentlyMaxLife())
            {
                EnableLifePrompt();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Rocket")
        {
            StartFadeOut();
        }
    }

    private GameObject alreadyRefueledPrompt;


    private void EnableLifePrompt()
    {
        lifePromptObj.SetActive(true);
        if (lifePromptText.color.a != 1f)
        {
            var tempColor = lifePromptText.color;
            tempColor.a = 1f;
            lifePromptText.color = tempColor;
        }
    }

    private void DisableLifePrompt()
    {
        lifePromptObj.SetActive(false);
    }

    private void StartFadeOut()
    {
        StartCoroutine(FadeOutPrompt(1f));
    }

    IEnumerator FadeOutPrompt(float fadeOutTime)
    {
        float alpha = lifePromptText.alpha;

        for (float time = 0f; time < fadeOutTime; time += Time.deltaTime / fadeOutTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, time));
            lifePromptText.color = newColor;
            yield return null;
        }
        DisableLifePrompt();
    }
}
    