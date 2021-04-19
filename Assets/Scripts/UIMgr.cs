using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class UIMgr : MonoBehaviour
{
    public static UIMgr inst;
    private void Awake()
    {
        inst = this;
    }
    //Timer 
    public Text timerText;
    public float time;

    public Text chitsCountTxt;
    public float numChit;

   
    
    public void LaunchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    //Quits out of program
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
