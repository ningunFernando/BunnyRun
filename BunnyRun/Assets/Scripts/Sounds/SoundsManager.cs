using System.Collections;
using System.Collections.Generic;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundsManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider music;
    [SerializeField] Slider sfx;

    private void Awake()
    {
        music.onValueChanged.AddListener(ControlMusicVolume);
        sfx.onValueChanged.AddListener(ControlEffectsVolume);


    }


    void Start()
    {
        Charge();
    }
    private void ControlEffectsVolume(float value)
    {
        mixer.SetFloat("VolumeSFX", Mathf.Log10(value)*20);
        PlayerPrefs.SetFloat("VolumeSFX", sfx.value);
    }
    private void ControlMusicVolume(float value)
    {
        mixer.SetFloat("VolumeMusic", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("VolumeMusic", music.value);
    }
    private void Charge()
    {
        music.value = PlayerPrefs.GetFloat("VolumeMusic", 0.75f);
        sfx.value = PlayerPrefs.GetFloat("VolumeSFX ", 0.75f);
        ControlEffectsVolume(sfx.value);
        ControlMusicVolume(music.value);

    }
}
