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
public class InteractableUI : Interactable
{
    Button button;

    public virtual void Awake()
    {
        base.Awake();
        button = GetComponent<Button>();
    }

    public override void OnInteract(GameObject player)
    {
        button.onClick.Invoke();
    }
}
