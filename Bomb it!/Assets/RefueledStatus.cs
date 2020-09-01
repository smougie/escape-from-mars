using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RefueledStatus : MonoBehaviour
{
    [SerializeField] GameObject refuelingPromptObject;
    [SerializeField] TextMeshProUGUI refuelingPromptText;
    private Missile missileRef;

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
        if (missileRef.AlreadyRefueled())
        {
            refuelingPromptObject.SetActive(true);
        }
    }

    private void DisableAlreadyRefueledPrompt()
    {
        refuelingPromptObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rocket"))
        {
            GetMissileRef(collision);
            DisplayAlreadyRefueledPrompt();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rocket") && missileRef.CanFade())
        {
            StartCoroutine(FadeOutPrompt(1f));
            print("starting fade out prompt");
        }
    }

    IEnumerator FadeOutPrompt(float fadeOutTime)
    {
        var alpha = refuelingPromptText.alpha;

        for (float time = 0f; time < fadeOutTime; time += Time.deltaTime / fadeOutTime)
        {
            Color alphaColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, time));
            refuelingPromptText.color = alphaColor;
            yield return null;
        }
    }
}
