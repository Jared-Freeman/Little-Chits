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

public class Interactable : MonoBehaviour
{
    

    public string focusText = "";

    private Outline focusHighlight;

    public virtual void Awake()
    {
        transform.gameObject.
        gameObject.layer = 10; 
        focusHighlight = gameObject.GetComponent<Outline>();
    }

    [System.Serializable]
    public class InteractionEvent : UnityEvent { }
    public InteractionEvent onInteraction = new InteractionEvent();

    public virtual void Interact(GameObject player)
    {
        onInteraction.Invoke();
    }

    public class FocusEvent : UnityEvent { }
    public FocusEvent onFocus = new FocusEvent();

    public virtual void Focus()
    {
        onFocus.Invoke();
    }

    public class FocusLostEvent : UnityEvent { }
    public FocusLostEvent onFocusLost = new FocusLostEvent();

    public virtual void FocusLost()
    {
        onFocusLost.Invoke();
    }
}
