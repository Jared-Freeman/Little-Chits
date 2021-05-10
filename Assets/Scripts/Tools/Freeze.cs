using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freeze : MonoBehaviour
{
    private float duration = 5f;
    private Vector3 holdPosition;

    private void Awake()
    {
        Freeze otherFreeze = transform.GetComponent<Freeze>();
        holdPosition = transform.position;

        Rigidbody rb = transform.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        if (duration < 0)
        {
            Destroy(this);
        }
        transform.position = holdPosition;
        duration -= Time.deltaTime;
    }
}
