using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author: Jared Freeman
//desc: creates and maintains a static list of all instantiated Chits in a scene. Useful for making global modifications or otherwise managing Chits!
//      currently one of these NEEDS to be instantiated for the level to track Chits. 
//      This could kinda be changed but honestly I think it's better to explicitely reinstantiate this to call Start() again.
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
    //Add and remove chits from our list when they spawn/despawn!
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
        chits_list.Clear(); //removes any chits tracked from a prev scene (any remaining pointers would be dangerous to use!)
    }
    #endregion
}
