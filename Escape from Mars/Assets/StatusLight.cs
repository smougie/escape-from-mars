using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusLight : MonoBehaviour
{
    [SerializeField] Material activeMaterial;
    [SerializeField] Material notActiveMaterial;
    private new Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void TurnOff()
    {
        renderer.material = notActiveMaterial;
    }

    public void TurnOn()
    {
        renderer.material = activeMaterial;
    }
}
