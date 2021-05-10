using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public int goal = 1;

    private int progress = 0;

    private Text statusUI;

    private void Awake()
    {
        statusUI = GetComponent<Text>();
    }

    private void MarkCompleted()
    {
        statusUI.color = new Color(0, 1, 0);
        Destroy(this);
    }

    private void UpdateProgress()
    {
        /*progressUI.text = progress.ToString() + "/" + goal.ToString();*/
    }


    public virtual void Increment()
    {
        progress += 1;
        if (progress >= goal)
        {
            progress = goal;
            MarkCompleted();
        }
        UpdateProgress();
    }
    public virtual void Decrement()
    {
        progress -= 1;
        if (progress < 0)
        {
            progress = 0;
        }
    }


}
