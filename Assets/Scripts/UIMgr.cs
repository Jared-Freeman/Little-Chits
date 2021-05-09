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
    public GameObject settingPanel;
    public GameObject controlPanel;
    public GameObject levelOverPanel;
    public GameObject taskPanel;


    public Slider masterVolSlider;
    public Slider musicVolSlider;
    public Slider chitVolSlider;
    public Slider footstepVolSlider;

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.gameObject.SetActive(!menuPanel.gameObject.activeSelf);
            MenuPopUpFunctions();
            if (SceneManager.GetActiveScene().name == "Level1" || SceneManager.GetActiveScene().name == "Level2")
            {
                menuButtonPanel.gameObject.SetActive(true);
                timerText2.gameObject.SetActive(true);
                mmWaningPanel.gameObject.SetActive(false);
                lbWaningPanel.gameObject.SetActive(false);
                settingPanel.gameObject.SetActive(false);
                controlPanel.gameObject.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            taskPanel.gameObject.SetActive(!taskPanel.gameObject.activeSelf);
        }
    }

    public void LaunchScene(string sceneName)
    {
        if(sceneName != "Leaderboard" || sceneName != "MainMenu")
        {
            Cursor.lockState = CursorLockMode.Locked;
            ResumeGame();
        }
        if (sceneName == "Level1" || sceneName == "Level2" || sceneName == "Level3")
        {
            Debug.Log("SHould Unlock");
            Cursor.lockState = CursorLockMode.Locked;
            ResumeGame();
        }
        if (sceneName == "Leaderboard" || sceneName == "MainMenu")
        {
            Cursor.lockState = CursorLockMode.None;
            ResumeGame();
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
            UnlockTheCursor();
        }
        else if (menuPanel.gameObject == !menuPanel.gameObject.activeSelf)
        {
            LockTheCursor();


        }
    }
     public void LevelOverAction()
    {
        levelOverPanel.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void LockTheCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        ResumeGame();
    }
    public void UnlockTheCursor()
    {
        PauseGame();
        Cursor.lockState = CursorLockMode.None;
    }
}
