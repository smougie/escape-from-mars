using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaygroundCameraControler : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    [SerializeField] Vector3 cameraPosition;
    [SerializeField] Vector3 cameraRotation;
    private Missile missileRef;
    public bool setCameraPosition;
    private Vector3 offsetPosition;

    void Start()
    {
        missileRef = GetComponent<Missile>();
        setCameraPosition = true;
        CalculateOffset();
        SetCameraOffset();
    }

    void Update()
    {
        if (setCameraPosition)
        {
            CalculateOffset();
            SetCameraOffset();
        }
    }

    private void CalculateOffset()
    {
        offsetPosition = new Vector3(transform.position.x, transform.position.y + cameraPosition.y, transform.position.z + cameraPosition.z);
    }

    private void SetCameraOffset()
    {
        mainCamera.transform.position = offsetPosition;
    }
}
