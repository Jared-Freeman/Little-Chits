using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */


public class GravityTool : Tool
{
    public InteractionSystem playerInteractionSystem;

    [Range(1, 10)]
    public float hoverDistanceFromCamera = 2;

    private Rigidbody otherRB;
    private GravityInterceptor gi;
    private GameObject cameraInterceptTarget;

    public void Awake()
    {
        base.Awake();
        focusText = "[E] to pickup gravity tool";

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

    public override void OnPickup(GameObject player)
    {
        playerInteractionSystem = player.GetComponentInChildren<InteractionSystem>();
    }

    public override void StartAction()
    {
        base.StartAction();
        Interactable interactable = playerInteractionSystem.focusedInteractable;
        if (interactable)
        {
            if (interactable is InteractableChit)
            {
                InteractableChit inter = (InteractableChit) playerInteractionSystem.focusedInteractable;
                inter.DisableChit();
            }
            gi = interactable.gameObject.AddComponent<GravityInterceptor>();
            gi.target = cameraInterceptTarget.transform;
        }
    }

    public override void EndAction()
    {

        Interactable interactable = playerInteractionSystem.focusedInteractable;

        base.EndAction();
        if (gi != null)
        {

            if (interactable is InteractableChit)
            {
                InteractableChit inter = (InteractableChit)playerInteractionSystem.focusedInteractable;
                inter.EnableChit();
            }

            Destroy(gi);
        }
    }
}