using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifePromptControler : MonoBehaviour
{
    [SerializeField] GameObject lifePromptObject;
    private TextMeshProUGUI lifePromptText;

    void Start()
    {
        lifePromptText = lifePromptObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void EnableLifePrompt()
    {
        lifePromptObject.SetActive(true);
        if (lifePromptText.color.a != 1f)
        {
            var tempColor = lifePromptText.color;
            tempColor.a = 1f;
            lifePromptText.color = tempColor;
        }
    }

    private void DisableLifePrompt()
    {
        lifePromptObject.SetActive(false);
    }

    public void StartFadeOut()
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
