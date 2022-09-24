using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public AudioMixer mixer;
    private const string MusicVolumeParameter = "musicVol";

    private void Start()
    {
        if (!PlayerPrefs.HasKey(MusicVolumeParameter))
            Save();

        Load();
    }

    public void ChangeVolumne(float sliderValue)
    {
        if (mixer == null)
            return;
        // Transform slider value to Db
        float sliderValueInDb = Mathf.Log10(sliderValue) * 20;

        mixer.SetFloat(MusicVolumeParameter, sliderValueInDb);
        Save();
    }

    private void Load()
    {
        if (mixer == null)
            return;

        ChangeVolumne(PlayerPrefs.GetFloat(MusicVolumeParameter));
    }

    private void Save()
    {
        if (mixer == null)
            return;

        if (mixer.GetFloat(MusicVolumeParameter, out float value))
            PlayerPrefs.SetFloat(MusicVolumeParameter, value);
    }

}