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
    [Range(1, 10)]
    public float hoverDistanceFromCamera = 2;

    private Rigidbody otherRB;
    private GravityInterceptor gi;
    private GameObject cameraInterceptTarget;

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

    public override void Pickup(GameObject player, InventorySystem inventory)
    {
        base.Interact(player);
        base.Pickup(player, inventory);
    }

    public override bool StartAction()
    {
        if (!base.StartAction())
            return false;

        if (interactable != null)
        {
            gi = interactable.gameObject.AddComponent<GravityInterceptor>();
            gi.target = cameraInterceptTarget.transform;
        }

        if (chit != null)
        {
            chit.Grabbed();
        }

        return true;
    }

    public override void EndAction()
    {
        base.EndAction();
        if (gi != null)
        {
            Destroy(gi);
        }

        if (chit != null)
        {
            chit.Released();
            chit = null;
        }
    }
}