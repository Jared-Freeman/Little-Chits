using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static event System.EventHandler<LeaderboardAttributesEventArgs> Event_GameOver;
    public int level_number = -1; 

    // Start is called before the first frame update
    void Start()
    {
        //prevent designer error of not setting level number in an instance
        if(level_number < 1)
        {
            Debug.LogError("ERROR: Please enter a valid level_number in Timer.cs script!");
        }

        //Timer 
        if (UIMgr.inst.timerText != null)
        {
            UIMgr.inst.time = 300; //5 minutes  //300
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
    }

    void Update()
    {
      
        if (UIMgr.inst.time <= 0) //0 minutes terminates
        {
            UIMgr.inst.LevelOverAction();
            DoGameOver();
        }
        UIMgr.inst.chitsCountTxt.text = UIMgr.inst.numChit.ToString("0"); ;

        //terminate program when you reach 0
        if (UIMgr.inst.numChit == 0)
        {
            UIMgr.inst.LevelOverAction();
            DoGameOver();
        }
    }

    void UpdateTimer()
    {
        if (UIMgr.inst.timerText != null)
        {
        UIMgr.inst.time -= Time.deltaTime;
            string minutes = Mathf.Floor(UIMgr.inst.time / 60).ToString("00");
            string seconds = (UIMgr.inst.time % 60).ToString("00");
        UIMgr.inst.timerText.text = minutes + ":" + seconds;
        UIMgr.inst.timerText2.text = minutes + ":" + seconds;
        }
    }

    //appended code by Jared
    //TODO: Make playername custom
    private void DoGameOver()
    {
        Event_GameOver?.Invoke(this, new LeaderboardAttributesEventArgs(new LeaderboardAttributes("Player", CalculateScore() ), level_number));
    }

    //appended code by Jared
    //dummy formula that may need changing...
    private int CalculateScore()
    {
        return (int)Mathf.Clamp((UIMgr.inst.time * 2 + (100 - (UIMgr.inst.numChit * 10))), 0, Mathf.Infinity);
    }
}
