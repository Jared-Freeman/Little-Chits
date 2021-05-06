using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardUpdateProxy : MonoBehaviour
{
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
        if (args.level - 1 >= 0 && args.level - 1 < LeaderboardStaticList.leaderboard_lists.Count)
        {
            LeaderboardStaticList.leaderboard_lists[args.level - 1].Add(args.la_arg);
        }
    }
    #endregion
}
