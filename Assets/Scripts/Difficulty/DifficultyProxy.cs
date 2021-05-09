using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyProxy : MonoBehaviour
{

    #region events
    public static event System.EventHandler<System.EventArgs> Event_Difficulty_Settings_Initialized;
    #endregion

    #region members
    public Timer timer;
    #endregion

    private void Start()
    {
        if(timer == null)
        {
            Debug.LogError("DifficultyProxy: ERROR, please define timer!");
        }

        UpdateDifficultySettings();
    }

    //should be done after map initialization. probably should be impl as event handler
    void UpdateDifficultySettings()
    {
        DifficultySetting d_setting = DifficultyManager.CurrentDifficultySetting(timer.level_number - 1); //off by one error fix because I was FOOLISH a while back

        timer.level_time = d_setting.level_time;

        Event_Difficulty_Settings_Initialized?.Invoke(this, new System.EventArgs());
    }
}
