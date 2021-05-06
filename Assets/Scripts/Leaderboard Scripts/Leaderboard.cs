using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

//@auth: Jared Freeman
//Idea: A leaderboard is a list of player initials and associated scores.
//Scores should be reported on level end. The leaderboard will then acquire the score and store it in the list.

//container to hold leaderboard attributes
public class LeaderboardAttributes
{
    public int score;
    public string player_name;
    
    public LeaderboardAttributes(string name, int scr)
    {
        player_name = name;
        score = scr;
    }
}

public class Leaderboard : MonoBehaviour
{

    #region MEMBERS
    public bool flag_debug;

    [SerializeField]
    private int current_level_index = 0;

    //for display in scene
    public Text display_cur_level;
    public Text display_gold_name;
    public Text display_gold_score;
    public Text display_silver_name;
    public Text display_silver_score;
    public Text display_copper_name;
    public Text display_copper_score;
    //public Text display_
    #endregion

    #region EVENTS
    #endregion

    #region EVENT SUBSCRIPTIONS

    #endregion

    #region EVENT HANDLERS

    #endregion

    #region INIT
    
    private void Start()
    {
        UpdateDisplayElements();
    }
    #endregion

    //debug method
    public void PrintLeaderboard()
    {
        int i = 1;
        string print_string = "";
        foreach(List<LeaderboardAttributes> attr_list in LeaderboardStaticList.leaderboard_lists)
        {
            print_string = "level " + i.ToString() + ": ";
            foreach (LeaderboardAttributes attr in attr_list)
            {
                print_string += "[" + attr.player_name + "-" + attr.score + "] ";
            }
            Debug.Log(print_string);
        }
    }

    public void IncrementCurrentLevelIndex()
    {
        current_level_index++;
        current_level_index = Mathf.Clamp(current_level_index, 0, LeaderboardStaticList.leaderboard_lists.Count-1);
        UpdateDisplayElements();
    }
    public void DecrementCurrentLevelIndex()
    {
        current_level_index--;
        current_level_index = Mathf.Clamp(current_level_index, 0, LeaderboardStaticList.leaderboard_lists.Count-1);
        UpdateDisplayElements();
    }

    //this works VV
    public void UpdateDisplayElements()
    {
        //if (flag_debug) PrintLeaderboard();
        
        if (LeaderboardStaticList.leaderboard_lists != null && LeaderboardStaticList.leaderboard_lists.Count > current_level_index) // fix edge case methinks
        {
            //fancy lambda expression that I only partially know how to use
            LeaderboardStaticList.leaderboard_lists[current_level_index] = LeaderboardStaticList.leaderboard_lists[current_level_index].OrderByDescending(x => x.score).ToList();
        }

        if (display_cur_level == null)
            return;

        string score_text = "Score: ";
        if (flag_debug)
        {
            display_cur_level.text = (current_level_index + 1).ToString();
            display_gold_name.text = "Jared";
            display_gold_score.text = score_text + "33";
        }
        else //ugly but it works
        {
            display_cur_level.text = (current_level_index + 1).ToString();

            if(LeaderboardStaticList.leaderboard_lists[current_level_index].Count > 0)
            {
                display_gold_name.text = LeaderboardStaticList.leaderboard_lists[current_level_index][0].player_name;
                display_gold_score.text = score_text + LeaderboardStaticList.leaderboard_lists[current_level_index][0].score.ToString();
            }
            else
            {
                display_gold_name.text = "";
                display_gold_score.text = "";
            }

            if (LeaderboardStaticList.leaderboard_lists[current_level_index].Count > 1)
            {
                display_silver_name.text = LeaderboardStaticList.leaderboard_lists[current_level_index][1].player_name;
                display_silver_score.text = score_text + LeaderboardStaticList.leaderboard_lists[current_level_index][1].score.ToString();
            }
            else
            {
                display_silver_name.text = "";
                display_silver_score.text = "";
            }


            if (LeaderboardStaticList.leaderboard_lists[current_level_index].Count > 2)
            {
                display_copper_name.text = LeaderboardStaticList.leaderboard_lists[current_level_index][2].player_name;
                display_copper_score.text = score_text + LeaderboardStaticList.leaderboard_lists[current_level_index][2].score.ToString();
            }
            else
            {
                display_copper_name.text = "";
                display_copper_score.text = "";
            }

        }
    }
}
