using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classes are for use in defining event arguments in inter-monobehaviour communications
//leverages c# events
public class MonobehaviourEventArgs : System.EventArgs
{
    public MonoBehaviour mono;

    public MonobehaviourEventArgs(MonoBehaviour mo)
    {
        mono = mo;
    }
}

public class LeaderboardAttributesEventArgs : System.EventArgs
{
    public LeaderboardAttributes la_arg;
    public int level;

    public LeaderboardAttributesEventArgs(LeaderboardAttributes a, int lvl_number)
    {
        la_arg = a;
        level = lvl_number;
    }
}

public class ChitAIEventArgs : System.EventArgs
{
    public ChitAI chit;

    public ChitAIEventArgs(ChitAI a)
    {
        chit = a;
    }
}
