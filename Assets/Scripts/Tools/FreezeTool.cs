using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Author: Bryan Dedeurwaerder
 * Project: Little Chits
 * 
 * Description: 
 */


public class FreezeTool : Tool
{

    [Range(1, 10)]
    public float hoverDistanceFromCamera = 2;
    public GameObject ice;
    public float duration = 10;

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

    IEnumerator Freeze(ChitAI chit)
    {
        chit.Grabbed();
        chit.body.velocity = Vector3.zero;
        GameObject newIce = Instantiate(ice, chit.transform);
        yield return new WaitForSeconds(duration);
        Destroy(newIce);
        chit.Released();
    }

    public override bool StartAction()
    {
        if (!base.StartAction())
            return false;
        
        if (chit != null)
        {
            StartCoroutine("Freeze", chit);
        }

        return true;
    }

    public override void EndAction()
    {
        base.EndAction();
    }
}