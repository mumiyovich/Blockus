using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float volume) 
    {
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume + 0.000001f) *20); 
    }

    public void SetMusicVolume(float volume) 
    {
        audioMixer.SetFloat("SoundVolume", Mathf.Log10(volume + 0.000001f) * 20);
    }

    public void SetSoundEffectVolume(float volume) 
    {
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume + 0.000001f) * 20);
    }





}
