using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezerControler : MonoBehaviour
{
    [SerializeField] GameObject SqueezerActuator;
    [SerializeField] float xAxisScaleTarget;
    [SerializeField] float pushingSpeed;
    [SerializeField] float movingBackSpeed;
    [SerializeField] float delayTriggerTime;



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "SqueezeObject")
        {
            StartCoroutine(PushObject());
        }
    }

    IEnumerator PushObject()
    {
        if (delayTriggerTime > 0)
        {
            yield return new WaitForSeconds(delayTriggerTime);
        }
        var difference = Mathf.Abs(SqueezerActuator.transform.localScale.x - xAxisScaleTarget);
        var actuatorLocalScale = SqueezerActuator.transform.localScale;

        for (float i = 0f; i < difference; i += Time.deltaTime * pushingSpeed)
        {
            actuatorLocalScale.x += Time.deltaTime * pushingSpeed;
            SqueezerActuator.transform.localScale = actuatorLocalScale;
            yield return null;
        }
        yield return null;

        for (float i = 0f; i < difference; i += Time.deltaTime * movingBackSpeed)
        {
            actuatorLocalScale.x -= Time.deltaTime * movingBackSpeed;
            SqueezerActuator.transform.localScale = actuatorLocalScale;
            yield return null;
        }
    }
}
