using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalyControl : MonoBehaviour
{
    [SerializeField] ParticleSystem centerSphereParticle;
    [SerializeField] ParticleSystem ringParticles;
    [SerializeField] ParticleSystem preExplosionParticles;
    [SerializeField] ParticleSystem explosionParticles;
    [SerializeField] ParticleSystem voidParticles;
    [SerializeField] ParticleSystem dissapearParticles;
    [SerializeField] GameObject voidSphereObj;
    [SerializeField] float delayTime;
    [SerializeField] float activeTime;
    [SerializeField] float pauseTime;

    void Start()
    {
        voidSphereObj.SetActive(false);
        if (delayTime > 0)
        {
            StartCoroutine(DelayStart());
        }
        else
        {
            StartCoroutine(StartAnomaly());
        }
    }

    private void PlayStartParticlesSequence()
    {
        centerSphereParticle.Play();
        ringParticles.Play();
        preExplosionParticles.Play();
        explosionParticles.Play();
    }

    private void PlayActiveParticlesSequence()
    {
        voidSphereObj.SetActive(true);
        voidParticles.Play();
    }

    private void PlayEndParticlesSequence()
    {
        voidParticles.Stop();
        dissapearParticles.Play();
        StartCoroutine(ScaleDownSphere());
    }

    IEnumerator StartAnomaly()
    {
        PlayStartParticlesSequence();
        yield return new WaitForSeconds(6.2f);  // wait for finishing start particles sequence (6.2 is duration of all start particles)
        PlayActiveParticlesSequence();
        yield return new WaitForSeconds(activeTime);
        PlayEndParticlesSequence();
        yield return new WaitForSeconds(pauseTime);
        StartCoroutine(StartAnomaly());
    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(StartAnomaly());
    }

    IEnumerator ScaleDownSphere()
    {
        Vector3 startScale = voidSphereObj.transform.localScale;
        float scaleDownTime = 1f;
        for (float currentTime = 0f; currentTime < scaleDownTime; currentTime += Time.deltaTime)
        {
            voidSphereObj.transform.localScale = Vector3.Lerp(voidSphereObj.transform.localScale, new Vector3(0, 0, 0), currentTime);
            yield return null;
        }
        voidSphereObj.SetActive(false);
        voidSphereObj.transform.localScale = startScale;
    }
}
