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
    private void OnEnable()
    {
        Timer.Event_GameOver += UpdateLeaderboardList;
    }
    private void OnDisable()
    {
        Timer.Event_GameOver -= UpdateLeaderboardList;
    }
    #endregion

    #region EVENT HANDLERS
    private void UpdateLeaderboardList(object sender, LeaderboardAttributesEventArgs args)
    {
        if (args.level-1 >= 0 && args.level-1 < LeaderboardStaticList.leaderboard_lists.Count)
        {
            LeaderboardStaticList.leaderboard_lists[args.level - 1].Add(args.la_arg);
        }
    }
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
        current_level_index = Mathf.Clamp(current_level_index, 0, LeaderboardStaticList.leaderboard_lists.Count);
        UpdateDisplayElements();
    }
    public void DecrementCurrentLevelIndex()
    {
        current_level_index--;
        current_level_index = Mathf.Clamp(current_level_index, 0, LeaderboardStaticList.leaderboard_lists.Count);
        UpdateDisplayElements();
    }
    //this works VV
    public void UpdateDisplayElements()
    {
        //if (flag_debug) PrintLeaderboard();

        if (LeaderboardStaticList.leaderboard_lists != null && LeaderboardStaticList.leaderboard_lists.Count > current_level_index) // fix edge case methinks
        {
            //fancy lambda expression that I only partially know how to use
            LeaderboardStaticList.leaderboard_lists[current_level_index].OrderBy(x => x.score);
        }

        string score_text = "Score: ";
        if (flag_debug)
        {
            display_cur_level.text = (current_level_index + 1).ToString();
            display_gold_name.text = "Jared";
            display_gold_score.text = score_text + "33";
        }
    }
}
