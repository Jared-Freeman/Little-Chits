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
    public float chitVolume;
    [Range(0f, 1)]
    public float footstepVolume;
    /*[Range(0f, 1)]
    public float chitJumpVolume;
    [Range(0f, 1)]
    public float chitHappyVolume;
    [Range(0f, 1)]
    public float chitBadVolume;
    [Range(0f, 1)]
    public float chitDeathVolume;
    [Range(0f, 1)]
    public float chitCagedVolume;
    [Range(0f, 1)]
    public float chitGrabbedVolume;*/

    public bool played = false;

    // Update is called once per frame
    void Update()
    {
        SetVolume("masterVol", masterVolume);
        SetVolume("musicVol", musicVolume);
        SetVolume("chitVol", chitVolume);
        SetVolume("footstepVol", footstepVolume);        
        /*SetVolume("jumpVol", chitJumpVolume);
        SetVolume("happyVol", chitHappyVolume);
        SetVolume("badVol", chitBadVolume);
        SetVolume("deathVol", chitDeathVolume);
        SetVolume("cagedVol", chitCagedVolume);
        SetVolume("grabbedVol", chitGrabbedVolume);*/
    }

    public void SetVolume(string exposedParam, float value)
    {
        audioMixer.SetFloat(exposedParam, thresholdVolume * (1f - value));
    }
    public void SetMasterVolume()
    {
        masterVolume = UIMgr.inst.masterVolSlider.value;
    }
    public void SetMusicVolume()
    {
        musicVolume = UIMgr.inst.musicVolSlider.value;
    }
    public void SetChitVolume()
    {
        chitVolume = UIMgr.inst.chitVolSlider.value;
    }
    public void SetFootStepVolume()
    {
        footstepVolume = UIMgr.inst.footstepVolSlider.value;
    }
}
