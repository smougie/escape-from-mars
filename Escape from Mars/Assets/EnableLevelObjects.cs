using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLevelObjects : MonoBehaviour
{
    [SerializeField] bool disableOnStart;
    [SerializeField] GameObject[] toEnable;

    void Start()
    {
        if (disableOnStart)
        {
            DisableGameObjects();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Rocket")
        {
            EnableGameObjects();
        }
    }

    private void DisableGameObjects()
    {
        foreach (var item in toEnable)
        {
            item.SetActive(false);
        }
    }

    private void EnableGameObjects()
    {
        foreach (var item in toEnable)
        {
            item.SetActive(true);
        }
    }
}
