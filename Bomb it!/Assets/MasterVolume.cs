using UnityEngine;

public class MasterVolume : MonoBehaviour
{
    void Update()
    {
        if (AudioListener.volume != OptionsValues.loadedMasterVolumeValue)
        {
            AudioListener.volume = OptionsValues.loadedMasterVolumeValue;
        }
    }
}
