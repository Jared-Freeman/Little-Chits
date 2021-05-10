using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskUI : MonoBehaviour
{
    public string objective;
    public int goal = 1;

    private int progress;

    private Text statusUI;
    private Text objectiveUI;
    private Text progressUI;

    private void Awake()
    {
        statusUI = transform.Find("Status").GetComponent<Text>();
        objectiveUI = transform.Find("Objective").GetComponent<Text>();
        progressUI = transform.Find("Progress").GetComponent<Text>();

        objectiveUI.text = objective;
        progressUI.text = progress.ToString() + "/" + goal.ToString();
        statusUI.text = "";
    }

    private void MarkCompleted()
    {
        statusUI.text = "✓";
    }

    private void UpdateProgress()
    {
        progressUI.text = progress.ToString() + "/" + goal.ToString();
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
