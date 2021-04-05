using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* VaultAspect:
 * Definitions:
 *  - Climbing: Moving such that the endpoint is on top of an object
 *  - Vaulting: Moving such that the endpoint is on the other side of an object (or objects?)
 */

    
public class VaultAspect : MoveAspect
{
    private bool isVaulting = false;
    private bool isClimbing = false;
    private Vector3 vaultTarget = Vector3.zero;

    //test stuff
    private float vaultHeightRange; // vaultHeightRange / 2 will be extended +/- from middle of player + offsetFromPlayerMiddle.
    private float offsetFromPlayerMiddle = 0f;
    public BoxCollider legalVaultVolume;
    public int numObjInVaultVolume;
    public Rigidbody rb;

    //public GameObject vaultSensor;
    //public BoxCollider checkerVolume;

    public override void InitializeMoveAspect()
    {
        if (moveSystem.GetComponent<Rigidbody>() != null)
        {
            rb = moveSystem.GetComponent<Rigidbody>();
        }
        else
        {
            rb = moveSystem.gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.mass = 100f;

        legalVaultVolume = moveSystem.gameObject.AddComponent<BoxCollider>();
        legalVaultVolume.isTrigger = true;

        //Vector3 c = moveSystem.controller.center;
        
        float r = moveSystem.controller.radius;
        float h = moveSystem.controller.height;
        float maxVaultDist = 1.8f;
        float vaultHeightRange = .75f;
        float vaultHeightOffset = .2f; //offset from center
        Vector3 c = moveSystem.controller.center;
        
        legalVaultVolume.size = new Vector3(
            r + r                               //aka diameter
            , vaultHeightRange                  //whatever height range we want. Should be bounded by player height though
            , maxVaultDist                      //whatever vault max distance we want.
            );

        legalVaultVolume.center = new Vector3(
                c.x                             //no reason to have an x offset from center
            ,   c.y + vaultHeightOffset         //arbitrary offset from center.
            ,   c.z + maxVaultDist - r + .01f   //tiny extra offset to prevent self-detection
            );
        


        /*
        vaultSensor = new GameObject("Vault Sensor");

        if (gameObject != null)
        {
            vaultSensor.transform.parent = gameObject.transform;
            vaultSensor.transform.localPosition = Vector3.zero;


            checkerVolume = vaultSensor.AddComponent<BoxCollider>();
            checkerVolume.isTrigger = true;

            checkerVolume.size = new Vector3(
                    r + r                           //aka diameter
                , h                               //player height
                , r + r                           //aka diameter
                );

            checkerVolume.center = new Vector3(
                    c.x                             //no reason to have an x offset from center
                , legalVaultVolume.center.y + (legalVaultVolume.size.y / 2) + (checkerVolume.size.y / 2)       //arbitrary offset from center.
                , c.z + maxVaultDist - r + .01f   //tiny extra offset to prevent self-detection
                );
        }
        */
    }



    private void OnTriggerEnter(Collider other)
    {
        numObjInVaultVolume++;
    }

    private void OnTriggerExit(Collider other)
    {
        numObjInVaultVolume--;
    }

    private void OnTriggerStay(Collider other)
    {
        /*  //for fun
        if (other.attachedRigidbody)
            other.attachedRigidbody.AddForce(Vector3.up * 10);
        */
    }



    public override void DoUpdate( )
    {
        if (numObjInVaultVolume < 0) numObjInVaultVolume = 0;
        

        if (isVaulting) //continue to override movement until vault is finished
        {
            ContinueVault(moveSystem);
        }
        else if (isClimbing)
        {
            ContinueClimb(moveSystem);
        }
        else //check if we can vault and see if player chooses to vault
        {
            //if any objects are in our vault volume we must determine vaultability else climbability
            //is there a waist-high object in front of us?
            if (numObjInVaultVolume > 0)
            {
                //USE BOX CAST! :(



                Vector3 dir = moveSystem.transform.forward;

                Vector3 hExt;
                hExt.x = moveSystem.controller.radius;
                hExt.y = moveSystem.controller.height / 2;
                hExt.z = moveSystem.controller.radius;

                Vector3 c;
                c.x = legalVaultVolume.center.x;
                c.y = legalVaultVolume.center.y + (legalVaultVolume.size.y / 2) + hExt.y;
                c.z = legalVaultVolume.center.z - (legalVaultVolume.size.z / 2) + hExt.z;

                int mask = 0;

                float maxDist = 0f;

                RaycastHit hit;

                if (Physics.BoxCast(
                        c
                    ,   hExt
                    ,   dir
                    ,   out hit
                    ,   transform.rotation
                    ,   maxDist
                    ,   mask
                        ))
                {
                    
                }
                else //else its clear
                {

                }

                Debug.Log("vault detect");

                /*
                checkerVolume = moveSystem.gameObject.AddComponent<BoxCollider>();

                checkerVolume.center = new Vector3(5,0,0);
                
                checkerVolume.size = new Vector3(
                      moveSystem.controller.radius + moveSystem.controller.radius
                    , moveSystem.controller.height
                    , moveSystem.controller.radius + moveSystem.controller.radius
                    );
                */


                //can it be vaulted over?
                // - Take highest point detected from waistcheck
                // - project line less than or equal to vault distance across object.
                //   - if we don't hit anything we can vault. check for space on other side.

                /* Alt:
                 * - Ghost player on other side of object AT vault distance away. This can be a bit higher or lower if the ground is uneven. 
                 * - if there was space to stand:
                 * - Have that ghost look backward and snap to the closest point it can stand away from the object. This is our new vault target. 
                 * - Draw line at max waist height to connect the two positions. Can we still vault? Do vault!
                 */
                 
                //otherwise, is there enough space that we can stand on it? 
            }//end if
        }//end else
    }//end DoUpdate()

    /* ContinueVault:
     * Override player movement until we reach the target. When we reach target, isVaulting is set to false.
     */
    void ContinueVault(MoveSystem moveSystem)
    {
        //project movement. Did we arrive at destination?
        //if yes, we set isVaulting to false.
        //if no, we continue overriding. Do nothing


        //moveSystem.OverrideDesiredMovement();
    }

    /* ContinueClimb:
     * Override player movement until we reach the target. When we reach target, isClimbing is set to false.
     */
    void ContinueClimb(MoveSystem moveSystem)
    {

    }


    /* ProbeVault
     * Recursive function. Either gonna have a max or use tail recursion or rework I dunno
     * 
     * Returns the local height value offset of best vault. 
     * Returns [some error code I havent figured out yet] upon failing.
     * Could be positive distance from bottom of height range. -1 would be the error code instead
     * 
     * If we ray collide with something prior to max distance
     * else we do not ray collide with anything
     * 
     * Need status of last cast (collidedLastTime?)
     * 
     * 2D rect cast:
     *  - Start on a collide with the vault height range
     *  - Begin casting at the bottom of height range, go up at some increment y.
     *  - The first 
     */
    float ProbeVault(float height, bool collidedLastCheck)
    {
        //do a 2D rectangular cast. First one is at top of height range
        //did we collide? 
        return height;
    }
}
