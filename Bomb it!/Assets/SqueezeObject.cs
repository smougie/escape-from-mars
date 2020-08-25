using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SqueezeObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "NozzleFire")
        {
            Destroy(gameObject, 2f);
            // add particles and shrink here
        }
    }
}
