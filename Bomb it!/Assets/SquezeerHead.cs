using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquezeerHead : MonoBehaviour
{
    [SerializeField] GameObject contactPoint;  // contact point between squeezer head and squeezer actuator
    private bool contactPointAvailable = false;

    void Start()
    {
        CheckContactPointStatus();
    }

    void Update()
    {
        if (contactPointAvailable)
        {
            MoveSqueezerHead();
        }
    }

    private void MoveSqueezerHead()
    {
        transform.position = contactPoint.transform.position;
    }

    private void CheckContactPointStatus()
    {
        if (contactPoint != null)
        {
            contactPointAvailable = true;
        }
        else
        {
            contactPointAvailable = false;
        }
    }
}
