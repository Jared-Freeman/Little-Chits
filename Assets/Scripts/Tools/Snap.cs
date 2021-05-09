using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snap : MonoBehaviour
{
    public GameObject snapTarget;
    public float snapDistance = 1f;
    public float debounceSeconds = 1f;
    public float holdSeconds = 1f;

    private Vector3 lastPos;
    private float debounceTimer = 0f;
    private float holdTimer = 0f;
    private Rigidbody rb;

    private void Awake()
    {
        lastPos = transform.position;
        rb = transform.GetComponent<Rigidbody>();
    }

    private void SnapToTarget()
    {
        transform.position = snapTarget.transform.position;
        transform.rotation = snapTarget.transform.rotation;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private bool InSnapDistance()
    {
        return snapDistance > Vector3.Distance(transform.position, snapTarget.transform.position);
    }

    private void Update()
    {
        if (lastPos != transform.position)
        {
            lastPos = transform.position;
            if (InSnapDistance())
            {
                if (holdTimer < holdSeconds)
                {
                    SnapToTarget();
                    debounceTimer = debounceSeconds;
                }
                else if (debounceTimer < 0)
                {
                    holdTimer = 0;
                }
            }
        }
        debounceTimer -= Time.deltaTime;
        holdTimer += Time.deltaTime;
    }
}
