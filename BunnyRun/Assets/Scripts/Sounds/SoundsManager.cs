using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;
    [SerializeField] Slider musicConfig;
    [SerializeField] Slider sfxConfig;
    public static SoundsManager instance;

    private bool updatingSliders = false;

    private void Awake()
    {
        music.onValueChanged.AddListener((value) => UpdateMusicVolume(value, true));
        musicConfig.onValueChanged.AddListener((value) => UpdateMusicVolume(value, false));

        sfx.onValueChanged.AddListener((value) => UpdateSFXVolume(value, true));
        sfxConfig.onValueChanged.AddListener((value) => UpdateSFXVolume(value, false));
        instance = this;
    }

    private void Start()
    {
        LoadVolumes();
    }

    private void UpdateMusicVolume(float value, bool isMainSlider)
    {
        if (updatingSliders) return; 

        updatingSliders = true;

        if (isMainSlider)
            musicConfig.value = value;
        else
            music.value = value;

        mixer.SetFloat("VolumeMusic", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("VolumeMusic", value);

        updatingSliders = false;
    }

    private void UpdateSFXVolume(float value, bool isMainSlider)
    {
        if (updatingSliders) return; 

        updatingSliders = true;

        if (isMainSlider)
            sfxConfig.value = value;
        else
            sfx.value = value;

        mixer.SetFloat("VolumeSFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("VolumeSFX", value);

        updatingSliders = false;
    }

    public void LoadVolumes()
    {
        float defaultVolume = 0.75f;

        float musicVolume = PlayerPrefs.GetFloat("VolumeMusic", defaultVolume);
        float sfxVolume = PlayerPrefs.GetFloat("VolumeSFX", defaultVolume);

        updatingSliders = true;
        music.value = musicConfig.value = musicVolume;
        sfx.value = sfxConfig.value = sfxVolume;
        updatingSliders = false;

        mixer.SetFloat("VolumeMusic", Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat("VolumeSFX", Mathf.Log10(sfxVolume) * 20);
    }
}
