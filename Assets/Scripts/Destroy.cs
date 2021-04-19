using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public GameObject destroyedVersion;

     void OnCollisionEnter(Collision collision)
    {
        if (collision.impactForceSum.magnitude > 7f)
        {
            ContactPoint contact = collision.contacts[0];
            Quaternion rotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 position = contact.point;
            Instantiate(destroyedVersion, position, transform.rotation);
            Destroy(this.gameObject);
        }
    }

}
