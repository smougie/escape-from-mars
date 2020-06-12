using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] [Range(0, 1)] float volumeLevel;
    [SerializeField] 

    void Start()
    {
        AudioListener.volume = volumeLevel;
    }
}
