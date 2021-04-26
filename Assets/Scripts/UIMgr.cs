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
    public Text timerText2;
    public float time;

    public Text chitsCountTxt;
    public float numChit;

    public GameObject menuPanel;
    public GameObject menuButtonPanel;
    public GameObject mmWaningPanel;
    public GameObject lbWaningPanel;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
            menuButtonPanel.gameObject.SetActive(true);
            timerText2.gameObject.SetActive(true);
            mmWaningPanel.gameObject.SetActive(false);
            lbWaningPanel.gameObject.SetActive(false);            
            MenuPopUpFunctions();
        }
    }

    public void LaunchScene(string sceneName)
    {
        if (sceneName != "Level1")
        {
            Cursor.lockState = CursorLockMode.None;
        }
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
    public void MenuPopUpFunctions()
    {        
        if (menuPanel.gameObject == menuPanel.gameObject.activeSelf)
        {
            PauseGame();
            Cursor.lockState = CursorLockMode.None;
        }
        else if (menuPanel.gameObject == !menuPanel.gameObject.activeSelf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            ResumeGame();
            
        }
    }
}
