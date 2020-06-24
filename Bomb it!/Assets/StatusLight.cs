using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusLight : MonoBehaviour
{
    [SerializeField] Material activeMaterial;
    [SerializeField] Material notActiveMaterial;
    private new Renderer renderer;
    private bool padActive = true;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void TurnOff()
    {
        padActive = false;
        renderer.material = notActiveMaterial;
    }

    public void TurnOn()
    {
        padActive = true;
        renderer.material = activeMaterial;
    }
}
