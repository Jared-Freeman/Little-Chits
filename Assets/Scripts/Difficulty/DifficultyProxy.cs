using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author: Jared Freeman
//desc:     applies difficulty settings to the current scene. Note there are some things that need to be referenced to actually do that
public class DifficultyProxy : MonoBehaviour
{

    #region events
    public static event System.EventHandler<System.EventArgs> Event_Difficulty_Settings_Initialized;
    #endregion

    #region members
    public Timer timer;
    private bool flag_debug = true;
    #endregion

    IEnumerator Start()
    {
        if(timer == null)
        {
            Debug.LogError("DifficultyProxy: ERROR, please define timer!");
        }

        yield return new WaitForSeconds(.2f); //wait for other stuff to init. might solve the race cond issue -- seems like it did checking the logs!
        UpdateDifficultySettings();
    }

    //should be done after map initialization. probably should be impl as event handler
    void UpdateDifficultySettings()
    {
        //this code might create race conditions (is the stuff referenced here fully initalized at this point????)

        DifficultySetting d_setting = DifficultyManager.CurrentDifficultySetting(timer.level_number - 1); //off by one error fix because I was FOOLISH a while back

        timer.level_time = d_setting.level_time;
        UIMgr.inst.numChit = d_setting.number_of_chits;

        int keep_chits = 0;
        List<ChitAI> chits_to_remove = new List<ChitAI>();

        if (flag_debug) Debug.Log("determining chit count difficulty");

        foreach(ChitAI chit in ChitStaticList.chits_list)
        {
            keep_chits++;

            if(keep_chits <= d_setting.number_of_chits)
            {
                //could do some work here like changing chit behavior difficulty
            }
            else
            {
                chits_to_remove.Add(chit);
            }
        }
        //a bit safer I think
        foreach(ChitAI chit in chits_to_remove)
        {
            Destroy(chit.gameObject);
        }


        Event_Difficulty_Settings_Initialized?.Invoke(this, new System.EventArgs());
    }
}
