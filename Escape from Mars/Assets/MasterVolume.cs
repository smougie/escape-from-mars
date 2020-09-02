using UnityEngine;

public class MasterVolume : MonoBehaviour
{
    void Update()
    {
        if (AudioListener.volume != MasterVolumeValue())
        {
            AudioListener.volume = MasterVolumeValue();
        }
    }

    private float MasterVolumeValue()
    {
        return PlayerPrefs.GetFloat(OptionsValues.masterVolumeStr);
    }
}
