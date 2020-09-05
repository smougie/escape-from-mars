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

        nozzleFireMain.duration = activeTime;
        prewarmFireMain.duration = prewarmTime;

        StartCoroutine(DelayCoroutine());
    }

    private void PlayPrewarmFire()
    {
        if (playingPrewarmFire)
        {
            return;
        }
        playingPrewarmFire = true;
        prewarmFireEffect.Play();
    }

    private void PlayNozzleFire()
    {
        if (playingNozzleFire)
        {
            return;
        }
        playingNozzleFire = true;
        nozzleFireEffect.Play();
    }

    private void StartPause()
    {
        if (playingPause)
        {
            return;
        }
        playingPause = true;
    }

    IEnumerator DelayCoroutine()
    {
        if (delayTime > 0)
        {
            yield return new WaitForSeconds(delayTime);
        }
        StartCoroutine(PlayFireNozzleEffect());
    }

    IEnumerator PlayFireNozzleEffect()
    {
        float period = prewarmTime + activeTime + pauseTime;
        float counter = period;

        while (true)
        {
            counter -= Time.deltaTime;
            if (counter <= period && !playingNozzleFire)
            {
                PlayPrewarmFire();
            }
            if (counter <= period - prewarmTime && !playingNozzleFire)
            {
                boxCollider.isTrigger = true;
                boxCollider.enabled = true;
                PlayNozzleFire();
            }
            if (counter <= period - prewarmTime - activeTime && !playingPause)
            {
                boxCollider.isTrigger = false;
                StartCoroutine(DisableColliderInNewFrame());
                // Pause in this period of time
            }
            if (counter <= 0)
            {
                counter = period;
                playingNozzleFire = false;
                playingPrewarmFire = false;
            }
            yield return null;
        }
    }

    IEnumerator DisableColliderInNewFrame()
    {
        yield return new WaitForFixedUpdate();
        boxCollider.enabled = false;
    }
}
