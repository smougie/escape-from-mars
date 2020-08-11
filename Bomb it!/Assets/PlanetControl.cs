using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetControl : MonoBehaviour
{
    [SerializeField] GameObject planet1, planet2, planet3;

    public void ActivatePlanets(int planetsCount)
    {
        switch (planetsCount)
        {
            case 1:
                planet1.SetActive(true);
                planet2.SetActive(false);
                planet3.SetActive(false);
                break;
            case 2:
                planet1.SetActive(true);
                planet2.SetActive(false);
                planet3.SetActive(true);
                break;
            case 3:
                planet1.SetActive(true);
                planet2.SetActive(true);
                planet3.SetActive(true);
                break;
            default:
                planet1.SetActive(false);
                planet2.SetActive(false);
                planet3.SetActive(false);
                break;
        }
    }
}
