using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//classes are for use in defining event arguments in inter-monobehaviour communications

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

    public LeaderboardAttributesEventArgs(LeaderboardAttributes a)
    {
        la_arg = a;
    }
}
