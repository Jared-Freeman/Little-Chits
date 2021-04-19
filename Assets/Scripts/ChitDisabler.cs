using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ChitDisabler : MonoBehaviour
{
    private Rigidbody rb;
    private ChitAI chitAI;
    private NavMeshAgent navAgent;
    private float timer = 2f;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        chitAI = GetComponent<ChitAI>();
        navAgent = GetComponent<NavMeshAgent>();
        DisableChit();
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

    public void FixedUpdate()
    {
        if (timer < 0)
        {
            if (rb.velocity.magnitude < 1f)
            {
                EnableChit();
                Destroy(this);
            }
        }

        timer -= Time.deltaTime;

    }
}
