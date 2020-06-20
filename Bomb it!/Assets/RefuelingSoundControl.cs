using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuelingSoundControl : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] private float delayTime = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        yield return new WaitForSeconds(delayTime);
        audioSource.Play();
    }

}
