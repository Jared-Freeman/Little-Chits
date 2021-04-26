using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChitAIEventArgs : System.EventArgs
{
    public ChitAI chit_ai;

    public ChitAIEventArgs (ChitAI c)
    {
        chit_ai = c;
    }
}
