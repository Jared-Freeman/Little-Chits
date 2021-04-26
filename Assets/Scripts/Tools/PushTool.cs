using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */


public class PushTool : Tool
{
    public InteractionSystem playerInteractionSystem;

    [Range(1, 10)]
    public float hoverDistanceFromCamera = 2;

    private Rigidbody otherRB;
    private GravityInterceptor gi;
    private GameObject cameraInterceptTarget;
    private Interactable interactable;
    private ToolInteractable toolInteractable;

    public void Awake()
    {
        base.Awake();

        Transform trans = Camera.main.transform.Find("CameraInterceptTarget");
        if (trans != null)
        {
            cameraInterceptTarget = trans.gameObject;
        } else
        {
            cameraInterceptTarget = new GameObject();
            cameraInterceptTarget.name = "CameraInterceptTarget";
            cameraInterceptTarget.transform.parent = Camera.main.transform;
            cameraInterceptTarget.transform.localPosition = new Vector3(0, 0, hoverDistanceFromCamera);
        }
    }

    public override void Pickup(GameObject player)
    {
        base.Interact(player);
        playerInteractionSystem = player.GetComponentInChildren<InteractionSystem>();
    }

    public override void StartAction()
    {
        base.StartAction();
        interactable = playerInteractionSystem.focusedInteractable;
        if (interactable)
        {
            if (interactable is ToolInteractable)
            {
                toolInteractable = (ToolInteractable) interactable;
                toolInteractable.onToolInteraction.Invoke();
            }
            gi = interactable.gameObject.AddComponent<GravityInterceptor>();
            gi.target = cameraInterceptTarget.transform;
        } else
        {
            EndAction();
        }
    }

    public override void EndAction()
    {
        base.EndAction();
        if (gi != null)
        {
            Destroy(gi);
        }
    }
}