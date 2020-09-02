using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefueledStatus : MonoBehaviour
{
    [SerializeField] GameObject refuelingPromptObject;
    [SerializeField] TextMeshProUGUI refuelingPromptText;
    private Missile missileRef;
    private bool coroutineStarted;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void GetMissileRef(Collision collision)
    {
        if (missileRef != null)
        {
            return;
        }
        else
        {
            missileRef = collision.gameObject.GetComponent<Missile>();
        }
    }

    private void DisplayAlreadyRefueledPrompt()
    {
        if (refuelingPromptText.alpha != 1f)
        {
            var newColor = refuelingPromptText.color;
            newColor.a = 1f;
            refuelingPromptText.color = newColor;
        }
        if (missileRef.AlreadyRefueled())
        {
            refuelingPromptObject.SetActive(true);
        }
    }

    private void DisableAlreadyRefueledPrompt()
    {
        refuelingPromptObject.SetActive(false);
    }

    private void ResetAlphaValue()
    {
        var newColor = refuelingPromptText.color;
        newColor.a = 1f;
        refuelingPromptText.color = newColor;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rocket"))
        {
            GetMissileRef(collision);
            if (!coroutineStarted)
            {
                DisplayAlreadyRefueledPrompt();
                StartCoroutine(FadeOutPrompt(2f, 1f));
            }
        }
    }

    IEnumerator FadeOutPrompt(float fadeOutTime, float delayTime)
    {
        coroutineStarted = true;
        yield return new WaitForSeconds(delayTime);
        var alpha = refuelingPromptText.alpha;

        for (float time = 0f; time < fadeOutTime; time += Time.deltaTime / fadeOutTime)
        {
            Color alphaColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, time));
            refuelingPromptText.color = alphaColor;
            yield return null;
        }
        DisableAlreadyRefueledPrompt();
        ResetAlphaValue();
        coroutineStarted = false;
    }
}
