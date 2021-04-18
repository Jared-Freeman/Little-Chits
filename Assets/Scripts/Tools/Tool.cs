using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */

public abstract class Tool : Interactable
{
    #region members

    public AudioClip pickupSound;
    public AudioClip putDownSound;
    public AudioClip actionSound;

    public float coolDown;

    private Vector3 defaultLocalScale;
    private Vector3 defaultLocalPos;
    private Vector3 defaultLocalRot;

    private Rigidbody rb;
    private Collider collider;
    private GameObject inventorySystem;

    private AudioSource audioSource;

    #endregion

    public override void Awake()
    {
        base.Awake();
        rb = transform.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

        collider = transform.GetComponent<Collider>();

        if (collider == null)
        {
            collider = transform.GetComponentInChildren<Collider>();
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnDrop()
    {
        EndAction();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        collider.enabled = true;

        OnUnequip();
    }

    public virtual void OnPickup(GameObject player)
    {
        rb.isKinematic = true;
        collider.enabled = false;

        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }
    }

    public void OnEquip()
    {
        rb.isKinematic = true;
        collider.enabled = false;

        transform.localPosition = new Vector3(0.545f, -.309f, 0.587f); // TODO calculate this better with camera viewport considerations
        transform.localEulerAngles = new Vector3(180, 90, 170);

        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }
    }

    public void OnUnequip()
    {
        EndAction();
        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }
    }

    public virtual void StartAction()
    {
        audioSource.clip = actionSound;
        audioSource.Play();
    }
    public virtual void EndAction()
    {
        audioSource.Stop();
    }


    public override void OnInteract(GameObject player)
    {
        InventorySystem invSys = player.GetComponentInChildren<InventorySystem>();
        if (invSys != null)
        {
            invSys.Pickup(this);
        }
    }
}
