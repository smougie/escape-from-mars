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
        if (audioSource.volume != BackgroundVolumeValue())
        {
            audioSource.volume = BackgroundVolumeValue();
        }
    }

    private float BackgroundVolumeValue()
    {
        return PlayerPrefs.GetFloat(OptionsValues.backgroundVolumeStr);
    }
}
