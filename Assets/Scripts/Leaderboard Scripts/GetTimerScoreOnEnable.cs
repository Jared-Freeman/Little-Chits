using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTimerScoreOnEnable : MonoBehaviour
{
    public Timer timer;
    public Text score_text;

    void Start()
    {
        if (timer == null) Destroy(this);
        else if (score_text == null) Destroy(this);
    }

    private void OnEnable()
    {
        score_text.text = timer.score.ToString();
    }
}
