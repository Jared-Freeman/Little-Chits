using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */


// Anything in the environment that can be interacted with.
public class InteractableChit : Interactable
{

    private Rigidbody rb;
    private ChitAI chitAI;
    private NavMeshAgent navAgent;

    public virtual void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        chitAI = GetComponent<ChitAI>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void DisableChit()
    {
        rb.isKinematic = false;
        chitAI.enabled = false;
        navAgent.enabled = false;
    }

    public void EnableChit()
    {
        rb.isKinematic = true;
        chitAI.enabled = true;
        navAgent.enabled = true;
    }

    public override void OnInteract(GameObject player)
    {

    }
}
