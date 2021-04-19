using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
            SceneManager.LoadScene("GameOver");
        }
        UIMgr.inst.chitsCountTxt.text = UIMgr.inst.numChit.ToString("0"); ;

        //terminate program when you reach 0
        if (UIMgr.inst.numChit == 0)
        {
            SceneManager.LoadScene("GameOver");
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
        }
    }

}
