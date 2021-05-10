using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
    private bool flag_debug = true;
    public AudioSource attached_confirmation_sound_easy;
    public AudioSource attached_confirmation_sound_medium;
    public AudioSource attached_confirmation_sound_hard;

    public void SetDifficultyEasy()
    {
        if (flag_debug) Debug.Log("easy");
        attached_confirmation_sound_easy.Play();
        DifficultyManager.current_difficulty = DifficultyManager.Difficulty.Easy;
    }
    public void SetDifficultyMedium()
    {
        if (flag_debug) Debug.Log("medium");
        attached_confirmation_sound_medium.Play();
        DifficultyManager.current_difficulty = DifficultyManager.Difficulty.Medium;
    }
    public void SetDifficultyHard()
    {
        if (flag_debug) Debug.Log("hard");
        attached_confirmation_sound_hard.Play();
        DifficultyManager.current_difficulty = DifficultyManager.Difficulty.Hard;
    }
}
