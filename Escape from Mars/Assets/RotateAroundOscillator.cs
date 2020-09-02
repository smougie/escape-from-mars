using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundOscillator : MonoBehaviour
{
    [SerializeField] bool rotate = true;
    [SerializeField] bool aroundSelf = true;
    [SerializeField] bool aroundTarget = false;
    [SerializeField] GameObject rotateAround;
    [SerializeField] Vector3 selfRotateVector;
    [SerializeField] float selfRotateSpeed = 100f;
    [SerializeField] Vector3 targetRotateVector;
    [SerializeField] float targetRotateSpeed = 100f;
    private Vector3 targetPosition;

    void Start()
    {
        if (aroundTarget)
        {
            GetTargetPosition();
        }
    }

    void Update()
    {
        if (rotate)
        {
            if (aroundSelf && aroundTarget)
            {
                ComplexRotation();
            }
            else if (aroundSelf)
            {
                SelfRotation();
            }
            else if (aroundTarget)
            {
                TargetRotation();
            }
        }
    }

    private void SelfRotation()
    {
        transform.RotateAround(transform.position, selfRotateVector, Time.deltaTime * selfRotateSpeed);
    }

    private void TargetRotation()
    {
        transform.RotateAround(rotateAround.transform.position, targetRotateVector, Time.deltaTime * targetRotateSpeed);
    }

    private void ComplexRotation()
    {
        TargetRotation();
        SelfRotation();
    }

    private void GetTargetPosition()
    {
        targetPosition = rotateAround.transform.position;
    }
}
