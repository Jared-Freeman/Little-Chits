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

    public InteractionSystem playerInteractionSystem;

    public AudioClip pickupSound;
    public AudioClip putDownSound;

    public AudioClip actionSound;

    public bool fullChargeRestart; // if the player needs to wait until the tool is fully recharged before using

    [Range(0f,1f)]
    public float charge = 1;
    [Range(0f, 1f)]
    public float discargeRate = 0.1f; // the rate of charge rate per second when using
    [Range(0f, 1f)]
    public float rechargeRate = 0.1f; // the rate of charge rate per second when not using

    private Vector3 defaultLocalScale;
    private Vector3 defaultLocalPos;
    private Vector3 defaultLocalRot;

    private Rigidbody rb;
    private Collider collider;
    private bool doingAction;

    protected Interactable interactable;
    private InventorySystem inventorySystem;

    protected ChitAI chit;

    #endregion

    public override void Awake()
    {
        base.Awake();

        focusText = "[E] to pickup " + gameObject.name;
        rb = transform.GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

        collider = transform.GetComponent<Collider>();

        if (collider == null)
        {
            collider = transform.GetComponentInChildren<Collider>();
        }
    }

    [System.Serializable]
    public class PickupEvent : UnityEvent { }
    public PickupEvent onPickup = new PickupEvent();

    public virtual void Pickup(GameObject player, InventorySystem inventory)
    {
        rb.isKinematic = true;
        collider.enabled = false;

        inventorySystem = inventory;

        if (pickupSound != null)
        {
            inventorySystem.audioSource.clip = pickupSound;
            inventorySystem.audioSource.Play();
        }

        playerInteractionSystem = player.GetComponentInChildren<InteractionSystem>();

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
            inventorySystem.audioSource.clip = pickupSound;
            inventorySystem.audioSource.Play();
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
            inventorySystem.audioSource.clip = pickupSound;
            inventorySystem.audioSource.Play();
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

    IEnumerator IncreasePitch()
    {
        for (; inventorySystem.audioSource.pitch < 1; inventorySystem.audioSource.pitch += 0.01f)
        {
            yield return new WaitForSeconds(.01f);
        }
        inventorySystem.audioSource.pitch = 1f;

    }

    IEnumerator DecreasePitch()
    {
        for (; inventorySystem.audioSource.pitch >= 0; inventorySystem.audioSource.pitch -= 0.01f)
        {
            yield return new WaitForSeconds(.01f);
        }
        inventorySystem.audioSource.pitch = 0f;
        inventorySystem.audioSource.Stop();
    }

    public virtual bool StartAction()
    {
        if (fullChargeRestart)
        {
            if (charge != 1f)
            {
                return false;
            }
        }
        doingAction = true;
        inventorySystem.audioSource.clip = actionSound;
        //inventorySystem.audioSource.pitch = 0;
        inventorySystem.audioSource.Play();
        //StartCoroutine("IncreasePitch");

        interactable = playerInteractionSystem.focusedInteractable;
        if (interactable)
        {
            if (interactable.tag == "Chit")
            {
                chit = interactable.GetComponent<ChitAI>();
            }
        }
        else
        {
            EndAction();
        }

        return true;
    }

    public virtual void ContinueAction()
    {
        charge -= discargeRate * Time.deltaTime;
        inventorySystem.audioSource.pitch = charge;
        if (charge < 0)
        {
            charge = 0;
            EndAction();
        }
    }

    public virtual void EndAction()
    {
        doingAction = false;
        inventorySystem.audioSource.Stop();
    }

    public virtual void Update()
    {
        if (!doingAction)
        {
            charge += rechargeRate * Time.deltaTime;
            if (charge > 1f)
            {
                charge = 1f;
            }
        }
    }
}
