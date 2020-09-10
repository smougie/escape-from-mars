using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptManager : MonoBehaviour
{
    [SerializeField] bool onExit;

    private void OnTriggerEnter(Collider other)
    {
        if (onExit)
        {
            return;
        }
        else
        {
            if (other.CompareTag("Rocket"))
            {
                DisablePromptObj();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (onExit)
        {
            if (other.CompareTag("Rocket"))
            {
                DisablePromptObj();
            }
        }
        else
        {
            return;
        }
    }

    void DisablePromptObj()
    {
        gameObject.SetActive(false);
    }
}
