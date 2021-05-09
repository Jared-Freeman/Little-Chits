using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author:   Jared Freeman

#region helper classes
//desc:     Container class for one difficulty setting. Multiple should exist per level, defined in DifficultySettings
//TODO: add more settings!
public class DifficultySetting
{
    public DifficultySetting(float time, int chit_count, float t_scale = 1)
    {
        level_time = Mathf.Abs(time);
        number_of_chits = chit_count;
        time_scale = Mathf.Abs(t_scale);
    }

    public float level_time; //time in seconds (yeah i dunno why i chose to use float either...)
    public float time_scale; //typically defined to be 1. Could scale slightly for easier/harder experience. Probably not gonna impl this one in time so just ignore
    public int number_of_chits;
}
//desc:     Container class for diffculty settings. Needs to be defined across all difficulties (easy, meduim...)
public class DifficultySettings
{
    public DifficultySettings(DifficultySetting easy, DifficultySetting medium, DifficultySetting hard) //hooray for auto garbage collection
    {
        setting_easy = easy;
        setting_medium = medium;
        setting_hard = hard;
    }

    public DifficultySetting setting_easy;
    public DifficultySetting setting_medium;
    public DifficultySetting setting_hard;
}
#endregion

//desc:     Static container for holding difficulty settings to init scenes to their proper settings. Can be globally get/set 
//note:     Need to clean up any weird edits we make (such as changing global timescale) when tweaking difficulty.
public static class DifficultyManager
{
    #region CONSTRUCTOR
    static DifficultyManager() //can change initialization values here
    {
        settings_level_1 = new DifficultySettings(
            new DifficultySetting(300, 3)          //easy
            , new DifficultySetting(120, 4)        //medium
            , new DifficultySetting(75, 6));      //hard

        settings_level_2 = new DifficultySettings(
            new DifficultySetting(300, 6)          //easy
            , new DifficultySetting(240, 8)        //medium
            , new DifficultySetting(180, 12));      //hard

        settings_level_3 = new DifficultySettings(
            new DifficultySetting(360, 10)          //easy
            , new DifficultySetting(300, 12)        //medium
            , new DifficultySetting(240, 18));      //hard
    }
    #endregion

    #region MEMBERS
    //define new scene DifficultySettings if needed here
    public static DifficultySettings settings_level_1;
    public static DifficultySettings settings_level_2;
    public static DifficultySettings settings_level_3;
    #endregion

}
