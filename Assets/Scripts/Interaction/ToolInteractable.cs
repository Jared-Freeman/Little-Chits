using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */


// Anything in the environment that can be interacted with.

public class ToolInteractable : Interactable
{

    public virtual void Awake()
    {
        base.Awake();
    }

    [System.Serializable]
    public class ToolInteractionEvent : UnityEvent { }
    public ToolInteractionEvent onToolInteraction = new ToolInteractionEvent();

    public virtual void ToolInteract(GameObject player)
    {
        onToolInteraction.Invoke();
    }

    [System.Serializable]
    public class ToolInteractionEndedEvent : UnityEvent { }
    public ToolInteractionEndedEvent onToolInteractionEnded = new ToolInteractionEndedEvent();

    public virtual void ToolInteractEnded(GameObject player)
    {
        onToolInteractionEnded.Invoke();
    }

}
