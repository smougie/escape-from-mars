using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodgate : MonoBehaviour
{
    [SerializeField] Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 endPosition;
    [SerializeField] float delayTime;
    [SerializeField] float closingSpeed;
    [SerializeField] float openingSpeed;
    [SerializeField] float lockedPauseTime;
    [SerializeField] float openedPauseTime;
    private bool gateClosed;
    private bool gateOpened;

    void Start()
    {
        startPosition = transform.position;
        AdjustEndPosition();
        if (delayTime > 0)
        {
            StartCoroutine(StartMovingDelay());
        }
        else
        {
            StartMovingGate();
        }
    }

    private void StartMovingGate()
    {
        gateClosed = true;
    }

    void Update()
    {
        if (gateClosed)
        {
            StartCoroutine(OpenTheGate());
        }
        if (gateOpened)
        {
            StartCoroutine(CloseTheGate());
        }
    }

    private void AdjustEndPosition()
    {
        endPosition = startPosition;
        if (targetPosition.x != 0)
        {
            endPosition.x = targetPosition.x;
        }
        if (targetPosition.y != 0)
        {
            endPosition.y = targetPosition.y;

        }
        if (targetPosition.z != 0)
        {
            endPosition.z = targetPosition.z;
        }
    }

    IEnumerator StartMovingDelay()
    {
        yield return new WaitForSeconds(delayTime);
        StartMovingGate();
    }

    IEnumerator OpenTheGate()
    {
        gateClosed = false;
        while (transform.position != endPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * openingSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(openedPauseTime);
        gateOpened = true;
    }

    IEnumerator CloseTheGate()
    {
        gateOpened = false;
        while (transform.position != startPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime * closingSpeed);
            yield return null;
        }
        yield return new WaitForSeconds(lockedPauseTime);
        gateClosed = true;
    }

}
