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
