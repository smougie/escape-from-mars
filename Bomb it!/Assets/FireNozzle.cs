using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNozzle : MonoBehaviour
{
    [SerializeField] float delayTime;
    [SerializeField] float prewarmTime;
    [SerializeField] float activeTime;
    [SerializeField] float pauseTime;
    [SerializeField] ParticleSystem nozzleFireEffect;
    [SerializeField] ParticleSystem prewarmFireEffect;
    private BoxCollider boxCollider;
    private float prewarmFireParticlesLifeTime = .1f;
    private float nozzleFireParticlesLifeTime = .4f;
    private bool nozzleFireTriggered;
    private bool playingNozzleFire;
    private bool pauseTriggered;
    private bool playingPause;
    private bool playingPrewarmFire;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        var nozzleFireMain = nozzleFireEffect.main;
        var prewarmFireMain = prewarmFireEffect.main;

        nozzleFireMain.duration = activeTime - nozzleFireParticlesLifeTime;
        prewarmFireMain.duration = prewarmTime - prewarmFireParticlesLifeTime;

        StartCoroutine(DelayCoroutine());
    }

    private void PlayPrewarmFire(float counter)
    {
        if (playingPrewarmFire)
        {
            return;
        }
        playingPrewarmFire = true;
        prewarmFireEffect.Play();
        print("triggering prewarm Fire " + counter);
    }

    private void PlayNozzleFire(float counter)
    {
        if (playingNozzleFire)
        {
            return;
        }
        playingNozzleFire = true;
        nozzleFireEffect.Play();
        print("triggering Nozzle Fire " + counter);
    }

    private void StartPause(float counter)
    {
        if (playingPause)
        {
            return;
        }
        playingPause = true;
        print("triggering pause " + counter);
    }

    IEnumerator DelayCoroutine()
    {
        if (delayTime > 0)
        {
            print("starting delay");
            yield return new WaitForSeconds(delayTime);
        }
        print("ending delay");
        StartCoroutine(PlayFireNozzleEffect());
    }

    IEnumerator PlayFireNozzleEffect()
    {
        float period = prewarmTime + activeTime + pauseTime;
        float counter = period;

        while (true)
        {
            counter -= Time.fixedDeltaTime;
            if (counter <= period && !playingNozzleFire)
            {

                PlayPrewarmFire(counter);
            }
            if (counter <= period - prewarmTime && !playingNozzleFire)
            {
                boxCollider.enabled = true;
                PlayNozzleFire(counter);
            }
            if (counter <= period - prewarmTime - activeTime && !playingPause)
            {
                boxCollider.enabled = false;
                nozzleFireEffect.Stop();
                StartPause(counter);
            }
            if (counter <= 0)
            {

                counter = period;
                playingNozzleFire = false;
                playingPrewarmFire = false;
                print("reseting counter & flags " + counter);
            }
            yield return null;
        }
    }
}
