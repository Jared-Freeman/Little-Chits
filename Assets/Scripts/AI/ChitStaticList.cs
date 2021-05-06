using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitStaticList : MonoBehaviour
{
    #region MEMBERS
    private bool flag_debug;

    public static List<ChitAI> chits_list;
    #endregion

    #region EVENTS
    #endregion

    #region EVENT SUBSCRIPTIONS
    private void OnEnable()
    {
        ChitAI.event_chit_spawn += OnChitSpawn;
        ChitAI.event_chit_despawn += OnChitDespawn;
    }
    private void OnDisable()
    {
        ChitAI.event_chit_spawn -= OnChitSpawn;
        ChitAI.event_chit_despawn -= OnChitDespawn;
    }
    #endregion

    #region EVENT HANDLERS
    private void OnChitSpawn(object o, ChitAIEventArgs args)
    {
        chits_list.Add(args.chit);
    }
    private void OnChitDespawn(object o, ChitAIEventArgs args)
    {
        chits_list.Remove(args.chit);
    }
    #endregion

    #region INIT
    private void Awake()
    {
        if (chits_list == null)
            chits_list = new List<ChitAI>();
        chits_list.Clear();
    }
    #endregion
}
