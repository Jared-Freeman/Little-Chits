using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMgr : MonoBehaviour
{
    public static SoundMgr inst;
    private void Awake()
    {
        inst = this;
    }

    private float thresholdVolume = -80f;

    public AudioMixer audioMixer;

    [Range(0f, 1)]
    public float masterVolume;
    [Range(0f,1)]
    public float musicVolume;
    [Range(0f, 1)]
    public float chitIdleVolume;
    [Range(0f, 1)]
    public float chitPlayingVolume;
    [Range(0f, 1)]
    public float chitBadVolume;
    [Range(0f, 1)]
    public float chitDeathVolume;
    [Range(0f, 1)]
    public float chitCageVolume;



    // Update is called once per frame
    void Update()
    {
        SetVolume("masterVol", masterVolume);
        SetVolume("musicVol", musicVolume);
        SetVolume("idleVol", chitIdleVolume);
        SetVolume("playVol", chitPlayingVolume);
        SetVolume("badVol", chitBadVolume);
        SetVolume("deathVol", chitDeathVolume);
        SetVolume("cageVol", chitCageVolume);
    }

    public void SetVolume(string exposedParam, float value)
    {
        audioMixer.SetFloat(exposedParam, thresholdVolume * (1f - value));
    }
}
