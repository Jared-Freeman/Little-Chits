using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author:   Jared Freeman

#region helper classes
//desc:     Container class for one difficulty setting. Multiple should exist per level, defined in DifficultySettings
public class DifficultySetting
{
    public DifficultySetting(int time, int chit_count, float t_scale = 1)
    {
        level_time = Mathf.Abs(time);
        number_of_chits = chit_count;
        time_scale = Mathf.Abs(t_scale);
    }

    public int level_time; //time in seconds
    public float time_scale; //typically defined to be 1. Could scale slightly for easier/harder experience. Probably not gonna impl this one in time so just ignore
    public int number_of_chits;
}
//desc:     Container class for diffculty settings. Needs to be defined across all difficulties (easy, medium...)
public class DifficultyCollection
{
    public DifficultyCollection(DifficultySetting easy, DifficultySetting medium, DifficultySetting hard) //hooray for auto garbage collection
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
    public enum Difficulty {Easy = 0, Medium = 1, Hard = 2};

    #region CONSTRUCTOR
    static DifficultyManager() //can change initialization values here
    {
        if(difficulty_settings_list == null)
        {
            difficulty_settings_list = new List<DifficultyCollection>();
        }
        difficulty_settings_list.Clear();

        //You can modify difficulty settings by simply changing the constructor args below! Note that for number of chits you NEED to have at LEAST that many chit instances in the scene!

        //level 1
        difficulty_settings_list.Add( new DifficultyCollection(
            new DifficultySetting(300, 3)          //easy
            , new DifficultySetting(120, 4)        //medium
            , new DifficultySetting(75, 6) ));      //hard

        //level 2
        difficulty_settings_list.Add( new DifficultyCollection(
            new DifficultySetting(300, 6)          //easy
            , new DifficultySetting(240, 8)        //medium
            , new DifficultySetting(180, 12) ));      //hard

        //level 3
        difficulty_settings_list.Add( new DifficultyCollection(
            new DifficultySetting(360, 10)          //easy
            , new DifficultySetting(300, 12)        //medium
            , new DifficultySetting(240, 18) ));      //hard
    }
    #endregion

    #region MEMBERS
    //define new scene DifficultySettings if needed here
    public static List<DifficultyCollection> difficulty_settings_list;
    public static Difficulty current_difficulty;
    #endregion

    public static DifficultySetting CurrentDifficultySetting(int level_id)
    {
        if (current_difficulty == Difficulty.Easy)
        {
            return difficulty_settings_list[level_id].setting_easy;
        }
        else if (current_difficulty == Difficulty.Medium)
        {
            return difficulty_settings_list[level_id].setting_medium;
        }
        else if (current_difficulty == Difficulty.Hard)
        {
            return difficulty_settings_list[level_id].setting_hard;
        }
        else
        {
            Debug.LogError("DifficultyManager: ERROR! No implementation exists for the selected difficulty! Did we go out of bounds?");
            return null;
        }
    }
}
