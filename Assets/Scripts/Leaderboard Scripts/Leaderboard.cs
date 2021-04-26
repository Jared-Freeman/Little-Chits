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

}

public class Leaderboard : MonoBehaviour
{

    #region MEMBERS
    public bool flag_debug;

    [SerializeField]
    private int current_level_index = 0;
    public static List<List<LeaderboardAttributes>> leaderboard_lists; //stored across scenes. Eventually want to initialize this with some text file data or smthn

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
        
    }
    private void OnDisable()
    {
        
    }
    #endregion

    #region EVENT HANDLERS
    private void UpdateLeaderboardList(object sender, LeaderboardAttributesEventArgs args)
    {

    }
    #endregion

    #region INIT
    private void Awake()
    {
        if(leaderboard_lists == null)
        {
            leaderboard_lists = new List<List<LeaderboardAttributes>>();
        }
    }
    #endregion

    //debug method
    public void PrintLeaderboard()
    {
        int i = 1;
        string print_string = "";
        foreach(List<LeaderboardAttributes> attr_list in leaderboard_lists)
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
        current_level_index = Mathf.Clamp(current_level_index, 0, leaderboard_lists.Count);
        UpdateDisplayElements();
    }
    public void DecrementCurrentLevelIndex()
    {
        current_level_index--;
        current_level_index = Mathf.Clamp(current_level_index, 0, leaderboard_lists.Count);
        UpdateDisplayElements();
    }
    //this works VV
    public void UpdateDisplayElements()
    {
        //if (flag_debug) PrintLeaderboard();

        /*        if (leaderboard_lists != null && leaderboard_lists.Count > current_level_index) ; // fix edge case methinks
                {
                    //fancy lambda expression that I only partially know how to use
                    leaderboard_lists[current_level_index].OrderBy(x => x.score);
                }*/

        string score_text = "Score: ";

        display_cur_level.text = (current_level_index + 1).ToString();
        display_gold_name.text = "Jared";
        display_gold_score.text = score_text + "33";
    }
}
