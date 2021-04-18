using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityInterceptor : MonoBehaviour
{
    public Transform target;
    Rigidbody rb = null;

    [Range(0f, 1f)]
    public float velocityInflucnce = .05f; // 0 wild behavior, 1 tame behavior

    private float lerpSpeed = 10f;

    private void Awake()
    {
        rb = transform.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target)
        {
            //rb.MovePosition(Vector3.Lerp(rb.position, target.position, Time.deltaTime * lerpSpeed));
            Vector3 diff = target.position - transform.position;

            rb.AddForce(diff - (rb.velocity * velocityInflucnce), ForceMode.VelocityChange);
        }
    }
}
