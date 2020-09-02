using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointFlag : MonoBehaviour
{
    GameObject flagObject;
    Quaternion startingRotation;
    Quaternion endingRotation;


    void Start()
    {
        flagObject = gameObject.transform.Find("FlagObject").gameObject;
        flagObject.SetActive(false);
        startingRotation = transform.rotation;
        endingRotation = Quaternion.Euler(0, 0, 0);
    }

    public void RaiseFlag()
    {
        flagObject.SetActive(true);
        StartCoroutine(RotateFlag());
    }

    void LowerFlag()
    {
        flagObject.SetActive(false);
    }

    IEnumerator RotateFlag()
    {
        float rotatingSpeed = .02f;
        while (transform.rotation.z <= -.01f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, endingRotation, rotatingSpeed);
            yield return null;
        }
    }
}
