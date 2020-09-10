using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Rocket"))
        {
            DisablePromptObj();
        }
    }

    void DisablePromptObj()
    {
        gameObject.SetActive(false);
    }
}
