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
        float pushingForwardVelocity = Time.deltaTime * pushingSpeed;
        float pullingBackVelocity = Time.deltaTime * movingBackSpeed;

        for (float i = 0f; i < difference; i += pushingForwardVelocity)
        {
            actuatorLocalScale.x += pushingForwardVelocity;
            SqueezerActuator.transform.localScale = actuatorLocalScale;
            yield return null;
        }
        yield return null;

        for (float i = 0f; i < difference; i += pullingBackVelocity)
        {
            actuatorLocalScale.x -= pullingBackVelocity;
            SqueezerActuator.transform.localScale = actuatorLocalScale;
            yield return null;
        }
    }
}
