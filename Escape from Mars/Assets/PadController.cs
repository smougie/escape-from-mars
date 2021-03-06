﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadController : MonoBehaviour
{
    [SerializeField] bool DisableAfterLaunch;
    [SerializeField] float fadeTime;
    private Renderer objectRenderer;
    private Material objectMaterial;
    private Color objectColorStart;
    private Color objectColorEnd;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectMaterial = objectRenderer.material;
        objectColorStart = objectMaterial.color;
        objectColorEnd = new Color(objectColorStart.r, objectColorStart.g, objectColorStart.b, 0);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Rocket" && DisableAfterLaunch)
        {
            StartCoroutine(fadeOutPad(objectColorStart, objectColorEnd, fadeTime));
        }
    }

    IEnumerator fadeOutPad(Color startColor, Color endColor, float duration)
    {
        for (float time = 0f; time < duration; time += Time.deltaTime)
        {
            float normalizedTime = time / duration;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, normalizedTime);
            yield return null;
        }
        PadActive(false);
        ResetPadColor();
    }

    public void PadActive(bool active)
    {
        gameObject.SetActive(active);
    }

    private void ResetPadColor()
    {
        gameObject.GetComponent<Renderer>().material.color = objectColorStart;
    }
}
