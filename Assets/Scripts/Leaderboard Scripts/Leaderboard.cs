using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Idea: A leaderboard is a list of player initials and associated scores.
//Scores should be reported on level end. The leaderboard will then acquire the score and store it in the list.
public class Leaderboard : MonoBehaviour
{

    #region MEMBERS
    #endregion
    #region EVENTS
    public static event System.EventHandler<MonobehaviourEventArgs> RequestAlignToCameraAnglesEvent;
    #endregion
    #region EVENT SUBSCRIPTIONS
    #endregion
    #region EVENT HANDLERS
    #endregion
    #region INIT
    private void Awake()
    {
    }
    #endregion



}
