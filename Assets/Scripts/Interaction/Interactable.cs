using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        gameObject.layer = 10; // TODO make layer masks setting more automated and user friendly
        focusHighlight = gameObject.GetComponent<Outline>();
    }

    public void OnFocus()
    {
/*        if (focusHighlight == null)
        {
            focusHighlight = gameObject.AddComponent<Outline>();
        } else
        {
            focusHighlight.enabled = true;
        }*/
    }

    public void OnFocusLost()
    {
/*        if (focusHighlight != null)
        {
            focusHighlight.enabled = false;
        }*/
    }

    public virtual void OnInteract(GameObject player)
    {

    }
}
