using UnityEngine;

public class BackgroundVolume : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource.volume != OptionsValues.loadedBackgroundVolumeValue)
        {
            audioSource.volume = OptionsValues.loadedBackgroundVolumeValue;
        }
    }
}
