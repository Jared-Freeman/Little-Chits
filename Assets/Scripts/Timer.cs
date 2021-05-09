using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    #region events
    public static event System.EventHandler<LeaderboardAttributesEventArgs> Event_GameOver;
    #endregion

    #region members
    bool flag_debug = false;
    public int level_number = -1;
    public int level_time = 300;
    #endregion

    #region event subscriptions
    private void OnEnable()
    {
        DifficultyProxy.Event_Difficulty_Settings_Initialized += BeginTimer;
    }
    private void OnDisable()
    {
        DifficultyProxy.Event_Difficulty_Settings_Initialized -= BeginTimer;
    }
    #endregion

    #region event handlers
    void BeginTimer(object o, System.EventArgs a)
    {
        //Timer 
        if (UIMgr.inst.timerText != null)
        {
            if (flag_debug) Debug.Log(level_time.ToString());
            UIMgr.inst.time = (float)level_time; //5 minutes  //300s  //parametrized by Jared (hope I didnt break anything!)
            InvokeRepeating("UpdateTimer", 0.0f, 0.01667f);
        }
        StartCheckingForGameOver();
    }
    #endregion

    #region init
    void Start()
    {
        //prevent designer error of not setting level number in an instance
        if(level_number < 1)
        {
            Debug.LogError("ERROR: Please enter a valid level_number in Timer.cs script!");
        }
    }
    #endregion

    //moved gameover check outside of Update() for more execution flow control
    private void StartCheckingForGameOver()
    {
        StartCoroutine(ContinueCheckForGameOver());
    }
    private IEnumerator ContinueCheckForGameOver()
    {
        while(true)
        {
            if (UIMgr.inst.time <= 0) //0 minutes terminates
            {
                UIMgr.inst.LevelOverAction();
                DoGameOver();
            }
            UIMgr.inst.chitsCountTxt.text = UIMgr.inst.numChit.ToString("0"); ;

            //terminate program when you reach 0
            if (UIMgr.inst.numChit <= 0)
            {
                UIMgr.inst.LevelOverAction();
                DoGameOver();
            }

            yield return null;
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
    //TODO: update to new formula
    private int CalculateScore()
    {
        return (int)Mathf.Clamp((UIMgr.inst.time * 2 + (100 - (UIMgr.inst.numChit * 10))), 0, Mathf.Infinity);
    }
}
