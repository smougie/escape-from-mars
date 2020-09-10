using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
{
    [SerializeField] GameObject rocket;
    [SerializeField] ParticleSystem effect;
    private Rigidbody rocketRb;
    private bool gravityActive;

    void Start()
    {
        gravityActive = false;
        rocketRb = rocket.GetComponent<Rigidbody>();
    }

    void Update()
    {
        ManageGravity();
    }

    private void ManageGravity()
    {
        if (gravityActive)
        {
            rocketRb.useGravity = true;
        }
        else
        {
            rocketRb.useGravity = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            SwitchGravityState();
            effect.Play();
        }
    }

    void SwitchGravityState()
    {
        gravityActive = !gravityActive;
    }
}
