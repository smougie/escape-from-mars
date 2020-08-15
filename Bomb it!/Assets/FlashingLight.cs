using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingLight : MonoBehaviour
{
    [SerializeField] float minIntensity;
    [SerializeField] float maxIntensity;
    [SerializeField] float flashSpeed;
    private float distance;
    private Light lightObject;


    void Start()
    {
        lightObject = GetComponent<Light>();
        distance = maxIntensity - minIntensity;
    }

    void Update()
    {
        lightObject.intensity = minIntensity + Mathf.PingPong(Time.time * flashSpeed, distance);

    }

}
