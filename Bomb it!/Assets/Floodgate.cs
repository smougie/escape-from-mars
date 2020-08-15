using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floodgate : MonoBehaviour
{
    [SerializeField] Vector3 targetPosition;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float moveSpeed;
    private float lockedPauseTime;
    private float openedPauseTime;

    void Start()
    {
        startPosition = transform.position;
        AdjustEndPosition();
    }

    private void AdjustEndPosition()
    {
        if (targetPosition.z != 0)
        {
            
        }
        if (targetPosition.y != 0)
        {

        }
        if (targetPosition.z != 0)
        {

        }
    }

    void Update()
    {
        
    }
}
