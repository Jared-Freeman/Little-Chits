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
    private bool flag_debug = true;

    [Range(1, 10)]
    public float hoverDistanceFromCamera = 2;
    public GameObject ice;
    public float duration = 10; // time added per second. a bit of a misnomer now honestly

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
            //StartCoroutine("Freeze", chit);

            FrozenChit attached_frozen_modifier = chit.GetComponent<FrozenChit>();
            if (attached_frozen_modifier == null)
            {
                attached_frozen_modifier = chit.gameObject.AddComponent<FrozenChit>();
                attached_frozen_modifier.ice = ice;
                attached_frozen_modifier.duration = duration * Time.deltaTime;
            }
            else
            {
                attached_frozen_modifier.duration += duration * Time.deltaTime;
            }

        }

        return true;
    }


    public override void ContinueAction()
    {
        base.ContinueAction();

        if(chit != null)
        {
            //slightly messy but EH whatever
            FrozenChit attached_frozen_modifier = chit.GetComponent<FrozenChit>();
            if (attached_frozen_modifier == null) //likely never to travel to this branch
            {
                attached_frozen_modifier = chit.gameObject.AddComponent<FrozenChit>();
                attached_frozen_modifier.ice = ice;
                attached_frozen_modifier.duration = duration * Time.deltaTime;
            }
            else
            {
                if (flag_debug) Debug.Log("adding frozen timer: " + attached_frozen_modifier.duration.ToString());
                attached_frozen_modifier.duration += duration * Time.deltaTime;
            }
        }
    }

    public override void EndAction()
    {
        base.EndAction();
    }
}