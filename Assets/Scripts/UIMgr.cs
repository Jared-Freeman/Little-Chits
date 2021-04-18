using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class UIMgr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
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
}
