using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeObject : MonoBehaviour
{
    [SerializeField] GameObject meltingEffect;
    [SerializeField] GameObject meltingEffectEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "NozzleFire")
        {
            float randomTimeValue = Random.Range(2f, 4f);
            StartCoroutine(SpawnEndEffect(randomTimeValue - .1f));
            Destroy(gameObject, randomTimeValue);

            GameObject meltingEffectObject = Instantiate(meltingEffect, gameObject.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
            meltingEffectObject.transform.parent = gameObject.transform;
        }
        if (other.transform.tag == "NozzleInstantFire")
        {
            Instantiate(meltingEffectEnd, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "NozzleFire")
        {
            Instantiate(meltingEffectEnd, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnEndEffect(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Instantiate(meltingEffectEnd, gameObject.transform.position, Quaternion.Euler(-90, 0, 0));

    }
}
