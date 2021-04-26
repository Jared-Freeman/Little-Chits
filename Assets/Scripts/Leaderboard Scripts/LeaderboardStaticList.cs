using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Jared Freeman
//Idea: Track leaderboard stats across entire game. Allow for easy entry additions via a static class
public static class LeaderboardStaticList
{
    public static int num_levels = 2; //hardcoded for now...
    public static List<List<LeaderboardAttributes>> leaderboard_lists; //stored across scenes. Eventually want to initialize this with some text file data or smthn

    //init
    static LeaderboardStaticList()
    {
        //create a list and add number of lists == number of levels
        if (leaderboard_lists == null)
        {
            leaderboard_lists = new List<List<LeaderboardAttributes>>();
            for (int i = 1; i < num_levels; i++)
            {
                leaderboard_lists.Add(new List<LeaderboardAttributes>());
            }
        }

    }

    public static void AddLeaderboardEntry(LeaderboardAttributes attr, int level)
    {

    }
}
