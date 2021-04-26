using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public float audioLoopStart;
    public float audioLoopEnd;

    public float coolDown;

    private Vector3 defaultLocalScale;
    private Vector3 defaultLocalPos;
    private Vector3 defaultLocalRot;

    private Rigidbody rb;
    private Collider collider;
    private GameObject inventorySystem;

    protected AudioSource audioSource;

    #endregion

    public override void Awake()
    {
        base.Awake();

        focusText = "[e] to pickup " + gameObject.name;
        rb = transform.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

        collider = transform.GetComponent<Collider>();

        if (collider == null)
        {
            collider = transform.GetComponentInChildren<Collider>();
        }

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    [System.Serializable]
    public class PickupEvent : UnityEvent { }
    public PickupEvent onPickup = new PickupEvent();

    public virtual void Pickup(GameObject player)
    {
        rb.isKinematic = true;
        collider.enabled = false;

        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }

        onPickup.Invoke();
    }

    public class DropEvent : UnityEvent { }
    public DropEvent onDrop = new DropEvent();

    public virtual void Drop()
    {
        EndAction();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        collider.enabled = true;

        OnUnequip();

        onDrop.Invoke();
    }

    public class EquipEvent : UnityEvent { }
    public EquipEvent onEquip = new EquipEvent();

    public virtual void Equip()
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

        onEquip.Invoke();
    }

    public class UnequipEvent : UnityEvent { }
    public UnequipEvent onUnequip = new UnequipEvent();

    public virtual void OnUnequip()
    {
        EndAction();
        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
        }

        onUnequip.Invoke();
    }
    public override void Interact(GameObject player)
    {
        base.Interact(player);

        InventorySystem invSys = player.GetComponentInChildren<InventorySystem>();
        if (invSys != null)
        {
            invSys.Pickup(this);
        }
    }

    public virtual void StartAction()
    {
        audioSource.clip = actionSound;
        audioSource.Play();
    }

    public virtual void ContinueAction()
    {
        
    }

    public virtual void EndAction()
    {
        audioSource.Stop();
    }
}
