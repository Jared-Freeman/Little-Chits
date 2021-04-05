using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAspect : MonoBehaviour
{
    //constructors
    public MoveAspect()
    {
        aspectEnabled = true; canBeInterrupted = false;
    }
    public MoveAspect(bool enableStatus)
    {
        aspectEnabled = enableStatus; canBeInterrupted = false;
    }

    [Header("Ref [Auto-Added]")]
    //ref
    public MoveSystem moveSystem;

    //settings

    [Header("General Aspect Settings")]
    [Tooltip("Whaddya think bud")]
    public bool aspectEnabled;

    //this one gets edited in the ctor of the child

    [Tooltip("Can other move aspects stop this one?")]
    public bool canBeInterrupted;


    //virtual methods
    public virtual void DoUpdate() { }
    public virtual void InitializeMoveAspect() { }    //Allows aspect to generate any additional components it needs once at start
    //next time we can use Broadcasting since these are Monobehaviors...
    public virtual void OnGroundHit() { }
    public virtual void OnAirtimeStart() { }
    public virtual bool IsEnabled() { return aspectEnabled; }
    public virtual void EnableAspect() { aspectEnabled = true; }
    public virtual void DisableAspect() { aspectEnabled = false; }

    //methods
    public void ToggleAspect()
    {
        if (aspectEnabled) DisableAspect();
        else EnableAspect();
    }

    //for use in MoveSystem Start()
    public void AttachMoveSystem()
    {
        if (GetComponent<MoveSystem>() != null)
            moveSystem = GetComponent<MoveSystem>();
        else
            Debug.LogError("Via ID: " + this.GetInstanceID() + " could not find MoveSystem ref!");
    }


}
