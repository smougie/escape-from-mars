﻿using UnityEngine;
using UnityEngine.UI;

public class OptionsValues : MonoBehaviour
{
    public static readonly string firstPlayStr = "FirstPlay";
    public static readonly string masterVolumeStr = "MasterVolume";
    public static readonly string backgroundVolumeStr = "BackgroundVolume";
    public static readonly string soundEffectsVolumeStr = "SoundEffectsVolume";
    private static int firstPlayInt;
    private static float defaulVolumeValue = .5f;

    [SerializeField] Slider masterVolumeSlider, backgroundVolumeSlider, soundEffectsSlider;
    [SerializeField] bool mainMenu;

    public static float loadedMasterVolumeValue;
    public static float loadedBackgroundVolumeValue;
    public static float loadedSoundEffectsVolumeValue;

    void Start()
    {
        if (mainMenu)
        {
            DisableOptionsUI();  // set options UI inactive after loading volumes level
        }
        print("Start Script: " + PlayerPrefs.GetInt(firstPlayStr));  // TODO delete print
        firstPlayInt = PlayerPrefs.GetInt(firstPlayStr);
        if (firstPlayInt == 0)
        {
            PlayerPrefs.SetInt(firstPlayStr, 1);
            print("First Play statement: " + PlayerPrefs.GetInt(firstPlayStr));  // TODO delete print
            SetDefaultVolume();
            ReadAudioValues();
            UpdateSliderValues();
        }
        else
        {
            print("!NOT! First Play statement: " + PlayerPrefs.GetInt(firstPlayStr));  // TODO delete print
            ReadAudioValues();
            UpdateSliderValues();
        }
    }

    void Update()
    {
        TrackVolumeChanges();
    }

    private void TrackVolumeChanges()
    {
        if (masterVolumeSlider.value != loadedMasterVolumeValue)
        {
            SetMasterVolume(masterVolumeSlider.value);
            LoadMasterVolume();
        }
        else if (backgroundVolumeSlider.value != loadedBackgroundVolumeValue)
        {
            SetBackgroundVolume(backgroundVolumeSlider.value);
            LoadBackgroundVolume();
        }
        else if (soundEffectsSlider.value != loadedSoundEffectsVolumeValue)
        {
            SetSoundEffectsVolume(soundEffectsSlider.value);
            LoadSoundEffectsVolume();
        }
    }

    private void UpdateSliderValues()
    {
        masterVolumeSlider.value = loadedMasterVolumeValue;
        backgroundVolumeSlider.value = loadedBackgroundVolumeValue;
        soundEffectsSlider.value = loadedSoundEffectsVolumeValue;
    }

    private void SetDefaultVolume()
    {
        SetMasterVolume(defaulVolumeValue);
        SetBackgroundVolume(defaulVolumeValue);
        SetSoundEffectsVolume(defaulVolumeValue);
    }

    private void ReadAudioValues()
    {
        LoadMasterVolume();
        LoadBackgroundVolume();
        LoadSoundEffectsVolume();
    }

    private void SetMasterVolume(float soundLevelValue)
    {
        PlayerPrefs.SetFloat(masterVolumeStr, soundLevelValue);
    }

    private void SetBackgroundVolume(float soundLevelValue)
    {
        PlayerPrefs.SetFloat(backgroundVolumeStr, soundLevelValue);
    }

    private void SetSoundEffectsVolume(float soundLevelValue)
    {
        PlayerPrefs.SetFloat(soundEffectsVolumeStr, soundLevelValue);
    }

    public void LoadMasterVolume()
    {
        loadedMasterVolumeValue = PlayerPrefs.GetFloat(masterVolumeStr);
    }

    public void LoadBackgroundVolume()
    {
        loadedBackgroundVolumeValue = PlayerPrefs.GetFloat(backgroundVolumeStr);
    }

    public void LoadSoundEffectsVolume()
    {
        loadedSoundEffectsVolumeValue = PlayerPrefs.GetFloat(soundEffectsVolumeStr);
    }

    private void DisableOptionsUI()
    {
        gameObject.SetActive(false);
    }
}
