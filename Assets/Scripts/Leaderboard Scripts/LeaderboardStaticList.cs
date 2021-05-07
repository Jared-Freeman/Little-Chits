using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Jared Freeman
//Idea: Track leaderboard stats across entire game. Allow for easy entry additions via a static class
public static class LeaderboardStaticList
{
    private static bool flag_dev = true;

    public static int num_levels = 2; //hardcoded for now...
    public static List<List<LeaderboardAttributes>> leaderboard_lists; //stored across scenes. Eventually want to initialize this with some text file data or smthn
    

    //init
    static LeaderboardStaticList()
    {
        //create a list and add number of lists == number of levels
        if (leaderboard_lists == null)
        {
            leaderboard_lists = new List<List<LeaderboardAttributes>>();
            for (int i = 0; i < num_levels; i++)
            {
                List<LeaderboardAttributes> new_list = new List<LeaderboardAttributes>();

                if(flag_dev) //just randomly seeding a few score values for fun
                {
                    new_list.Add(new LeaderboardAttributes("Quinn", 10 + (int)(10 * Random.value)));
                    new_list.Add(new LeaderboardAttributes("Elizabeth", 10 + (int)(10 * Random.value)));
                    new_list.Add(new LeaderboardAttributes("Bryan", 10 + (int)(10 * Random.value)));
                    new_list.Add(new LeaderboardAttributes("Jared", 10 + (int)(10 * Random.value)));
                }

                leaderboard_lists.Add(new_list);
            }
        }

    }

    public static void AddLeaderboardEntry(LeaderboardAttributes attr, int level)
    {
        if (level - 1 >= 0 && level - 1 < leaderboard_lists.Count)
        {
            leaderboard_lists[level - 1].Add(attr);
        }
    }
}
