using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelOverLeaderboardSubmit : MonoBehaviour
{
    public InputField inputfield_name;
    public int score;
    public int level = -1;

    private void Start()
    {
        if (level < 1)
            Debug.LogError("LevelOverLeaderboardSubmit: Please set the level!");
    }

    public void SubmitLeaderboardAttributes()
    {
        LeaderboardStaticList.AddLeaderboardEntry(new LeaderboardAttributes(inputfield_name.text, score), level);
    }

}
