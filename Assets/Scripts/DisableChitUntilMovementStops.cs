using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DisableChitUntilMovementStops : MonoBehaviour
{
    [Range(.01f, 20f)]
    public float secondsBeforeEnableAttempt = 2f;

    [Range(0.01f, 5f)]
    public float minSpeedBeforeEnable = 1f;

    private Rigidbody rb;
    private ChitAI chitAI;
    private NavMeshAgent navAgent;
    private float timer = 0;
    private bool active = false;

    public void Awake()
    {
        rb = GetComponent<Rigidbody>();
        chitAI = GetComponent<ChitAI>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void Disable()
    {
        rb.isKinematic = false;
        chitAI.enabled = false;
        chitAI.isGrabbed = true;
        chitAI.isObsessed = false;
        navAgent.enabled = false;
        active = true;
        timer = secondsBeforeEnableAttempt;
    }

    private void Enable()
    {
        rb.isKinematic = true;
        chitAI.enabled = true;
        chitAI.isGrabbed = false;
        navAgent.enabled = true;
        active = false;
    }

    public void FixedUpdate()
    {
        if (active)
        {
            if (timer < 0)
            {
                if (rb.velocity.magnitude < minSpeedBeforeEnable)
                {
                    Enable();
                }
            }
            timer -= Time.deltaTime;
        }
    }
}
