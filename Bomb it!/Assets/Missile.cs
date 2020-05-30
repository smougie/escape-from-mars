using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProccesInput();        
    }

    private void ProccesInput()
    {
        if (Input.GetKey(KeyCode.Space))  // can adjust speed while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up);
        }

        // reacting only to one statement at time
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {

        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {

        }
    }
}
