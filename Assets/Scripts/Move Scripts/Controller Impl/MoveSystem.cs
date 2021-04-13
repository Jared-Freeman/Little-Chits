using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[RequireComponent(typeof(CharacterController))]

/* MoveSystem:
 * Aspects will transmit their desired movement to the desiredMovement vector, or modify the camera position via its reference. 
 * MoveSystem SHOULD authoritatively Move() the controller (currently all CAN but this could be changed if we wanted)
 * 
 * Currently gravity is implemented in JumpAspect LOL
 */

public class MoveSystem : MonoBehaviour
{
    #region public members
    //member refs
    [Header ("References (Auto-Added)")]
    public CharacterController controller;
    public Camera playerCamera;
    public List<MoveAspect> aspectList;
    public CapsuleCollider playerCollider;
   
    [Space(5)]

    //member consts (for now)

    //[Header ("Gravity Settings")]
    public readonly float maxFallSpeed = 24f;
    public readonly float gravity = -9.81f;
    public readonly float gravityScale = 2f;
    //[Space(2)]

    public bool groundedAtCenter = false;
    public bool groundedAnywhere = false;
    #endregion

    #region private members
    //private members
    private float dynamicSpeedMultiplier = 1f;          //range of 0-1. Final mult laid onto move command

    private float groundedDistanceTolerance = .125f;      //experimentally derived value. Changing this would reduce "snap to ground" behavior
    private float playerHeight;                         //may be nicer ways to do this one
    private Vector3 desiredMovement = Vector3.zero;     //movement relative to current position
    private bool movementOverrideEvent = false;         //implementation for a movement interrupt event to be called by MoveAspects
    private Vector3 positionLastFrame = Vector3.zero;   // controller velocity not properly tracking. This aids the workaround
    [SerializeField]
    private Vector3 velocity = Vector3.zero;            // ^ duhdoi
    [SerializeField]
    private bool isGrounded = false;
    private Vector3 playerCameraOriginalLocalPosition;  //maybe hide later so modding cant happen outside here.
    private Vector3 playerCameraLocalPositionLastFrame;
    private Quaternion playerCameraRotationReturn;      //what to return to 
    private Quaternion playerCameraLocalRotationLastFrame;
    private Quaternion rotationLastFrame;

    //duh doi
    private bool groundedLastFrame = false;             //to check for a ground hit event

    //tracking hitting ground and entering airtime 
    private float timeBeforeLegalFall = .25f;           //time in seconds before being off the ground counts as falling
    private float fallTime = 0f;
    private float timeBeforeLegalAirtime = .25f;
    private float groundTime = 0f;
    [SerializeField]
    private bool noGroundSnap = false;

    [SerializeField]
    private bool isNearGround = false;
    #endregion

    #region trivial methods
    public bool IsMoving()
    {
        return velocity.sqrMagnitude > .05f;
    }
    public bool IsGrounded()
    {
        return isGrounded;
    }
    //We could add universal movement rules here if that helps. I dunno yet.
    public void AppendDesiredMovement(Vector3 move) { desiredMovement += move; }
    //Also dunno if we need this yet
    public void OverrideDesiredMovement(Vector3 move)
    {
        desiredMovement = move;
        movementOverrideEvent = true;
    }
    private bool MovementOverride()
    {
        if (movementOverrideEvent)
        {
            movementOverrideEvent = false; //reset override
            return true;
        }
        return false;
    }

    private void UpdateVelocity()
    {
        velocity = (controller.transform.position - positionLastFrame) / Time.deltaTime;
        positionLastFrame = controller.transform.position;
    }
    public Vector3 GetVelocity() { return velocity; }
    public Vector3 GetCameraOriginalPosition() { return playerCameraOriginalLocalPosition; }
    public Quaternion GetCameraOriginalRotation() { return playerCameraRotationReturn; }
    void UpdateCameraRotationReturn()
    {
        //maybe???
        Vector3 oldAngles = playerCameraRotationReturn.eulerAngles;
        playerCameraRotationReturn.eulerAngles = new Vector3(oldAngles.x, controller.transform.rotation.eulerAngles.y, oldAngles.z);
    }

    //allow an accessor to start overriding movement externally
    public void StartOverridingMovement() { movementOverrideEvent = true; }
    public void StopOverridingMovement() { movementOverrideEvent = false; }

    public float GetHeight() { return controller.height; }
    public float GetRadius() { return controller.radius; }
    public Vector3 GetCenter() { return controller.center; }
    public bool IsLegalFall() { return fallTime >= timeBeforeLegalFall; }
    public bool IsLegalAirtime() { return groundTime >= timeBeforeLegalAirtime; }

    public void SetDynamicSpeedMultiplier(float x) { dynamicSpeedMultiplier = x; dynamicSpeedMultiplier = Mathf.Clamp(dynamicSpeedMultiplier, 0f, Mathf.Infinity); }
    public void AppendDynamicSpeedMultiplier(float x) { dynamicSpeedMultiplier += x; dynamicSpeedMultiplier = Mathf.Clamp(dynamicSpeedMultiplier, 0f, Mathf.Infinity); }

    public bool AddAspect(MoveAspect ma)
    {

        return true;
    }
    public void RemoveAllAspects()
    {
        foreach (MoveAspect aspect in aspectList)
        {
            GameObject.Destroy(aspect);
        }
    }
    #endregion

    //methods
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Stay");

        Vector3 diff = transform.position - collision.transform.position;
        diff.Normalize();

        collision.rigidbody.AddForce(diff * 1000f * Time.deltaTime);
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerHeight = Mathf.Abs(controller.height); //probably dont need to abs.
        positionLastFrame = controller.transform.position;
        rotationLastFrame = controller.transform.rotation;

        if (playerCamera == null && GetComponentInChildren<Camera>() != null) //allow assigning this outside of gameObject hierarchy (i.e. as sibling)
        {
            playerCamera = GetComponentInChildren<Camera>();
            playerCameraOriginalLocalPosition = playerCamera.transform.localPosition;
            playerCameraRotationReturn = playerCamera.transform.rotation;
            playerCameraLocalPositionLastFrame = playerCameraOriginalLocalPosition;
            playerCameraLocalRotationLastFrame = playerCamera.transform.localRotation;
        }
        else
        {
            Debug.LogWarning("Custom Camera Init on MoveSystem! Was this intentional? Changing to null! :P");
            playerCamera = null;
        }

        aspectList = GetComponents<MoveAspect>().ToList();
        foreach (MoveAspect aspect in aspectList)
        {
            aspect.AttachMoveSystem();
            aspect.InitializeMoveAspect();
        }

        UpdateIsGrounded();
        groundedLastFrame = isGrounded;

        if(controller.stepOffset <= controller.radius)
            controller.stepOffset = controller.radius + .001f; //This prevents jitter when pushing against stuff

        //Debug.Log(playerCameraRotationReturn + ", " + playerCameraRotationReturn.eulerAngles);
    }

    private void Update()
    {
        //reset desired movement every frame
        if (!movementOverrideEvent) SetDynamicSpeedMultiplier(1f);
        if (velocity.y > 0f) noGroundSnap = true;

        //each aspect applies its desired movement. 
        //Aspects will eventually be able to also override desired movement. 
        //Aspects can be unoverridable (like headbobbing).
        foreach (MoveAspect aspect in aspectList)
        {
            //only update if the aspect is enabled AND cannot be interrupted OR no override is happening rn
            if (aspect.IsEnabled() && (!aspect.canBeInterrupted || !movementOverrideEvent)) {

                if (groundedLastFrame != isGrounded && isGrounded && fallTime > timeBeforeLegalFall)
                    aspect.OnGroundHit();
                else if (groundedLastFrame != isGrounded && !isGrounded && groundTime > timeBeforeLegalAirtime)
                    aspect.OnAirtimeStart();

                aspect.DoUpdate();
            }
        }

        //if ((controller.collisionFlags & CollisionFlags.Sides) != 0) {}
        if(dynamicSpeedMultiplier != 1f) //only modify the xz stuff. y is involved in gravity and so forth
        {
            desiredMovement.x *= dynamicSpeedMultiplier;
            desiredMovement.z *= dynamicSpeedMultiplier;
        }

        #region removed cam interp code
        /*
           //interp player cam a bit CAN smooth out rapid movement (maybe not ideal though)
           playerCamera.transform.localPosition = Vector3.Lerp(playerCameraLocalPositionLastFrame
               , playerCameraOriginalLocalPosition
               , 1 - Mathf.Pow(.1f, Time.deltaTime));
           playerCamera.transform.localRotation = Quaternion.Lerp(playerCameraLocalRotationLastFrame
               , playerCamera.transform.localRotation
               , 1 - Mathf.Pow(.1f, Time.deltaTime));
         */
        #endregion

        Vector3 groundNormal = Vector3.zero;
        //if near ground, project move vector x/z on plane?
        if (!groundedAtCenter && groundedAnywhere)
        {
            isNearGround = true;
            NearGroundVector(ref groundNormal);
            Vector3 temp = Vector3.ProjectOnPlane(desiredMovement, groundNormal);
            desiredMovement.x = temp.x;
            desiredMovement.z = temp.z;
            desiredMovement.y += temp.y;
        }
        else isNearGround = false;
        controller.Move(desiredMovement);
        desiredMovement = Vector3.zero;

        UpdateMoveSystem();

        //if (isGrounded) noGroundSnap = false;

        //Debug.Log("Moving: " + IsMoving() + " Grounded: " + IsGrounded());

        //could do network position and rotation updates here after all aspects handle their shit.
    }

    void UpdateMoveSystem()
    {

        //Recalculate player movement statuses
        UpdateVelocity();

        groundedLastFrame = isGrounded;

        if (!isGrounded)
        {
            if (fallTime <= timeBeforeLegalFall) fallTime += Time.deltaTime;
            groundTime = 0f;
        }
        else
        {
            if (groundTime <= timeBeforeLegalAirtime) groundTime += Time.deltaTime;
            fallTime = 0f;
        }


        UpdateIsGrounded();
        UpdateCameraRotationReturn();
        playerCameraLocalPositionLastFrame = playerCamera.transform.localPosition;
        playerCameraLocalRotationLastFrame = playerCamera.transform.localRotation;
        rotationLastFrame = controller.transform.rotation;
    }

    //TODO: fix ground snapping thingy
    private void UpdateIsGrounded()
    {
        groundedAtCenter = GroundedAtCenter();
        groundedAnywhere = GroundedAnywhere();

        //if (desiredMovement.y > 0) isGrounded = false;
        //else 
        if (groundedAtCenter) isGrounded = true;
        else if (groundedAnywhere) isGrounded = true;
        else isGrounded = false;
        /*
        //NOTE: may want to check this but maybe not. Could do a vert speed check within the Aspect/accessor to this method
        //OK isGrounded is shit I hate it, we're removing it
        //if (controller.isGrounded) { isGrounded = true; return; } //mind-bogglingly inconsistent!
        //if (controller.velocity.y > 1f) { isGrounded = false; return; }

        Vector3 bt = controller.transform.position;
        bt.y -= (playerHeight / 2);

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0;

        float predictiveOffset = (horizontalVelocity.magnitude * Time.deltaTime);
        predictiveOffset = Mathf.Clamp(predictiveOffset, 0f, maxFallSpeed * Time.deltaTime);

        Debug.DrawRay(bt, (Vector3.down * groundedDistanceTolerance), Color.blue);
        RaycastHit hit;

        //if (noGroundSnap) { isGrounded = false; return; }

        if (Physics.Raycast(bt, Vector3.down, out hit, groundedDistanceTolerance))
        {
            if (!movementOverrideEvent)
            {

                //AppendDesiredMovement(new Vector3(0, -2 * Time.deltaTime - predictiveOffset, 0));// any reason this is on the heap? : Destroyed via OOS nearly instantly. -Jared
                isGrounded = true;

            }
            else isGrounded = true;
            
            //Debug.Log("raycast got a true");
            return;
        }


        //A more expensive check to see if we hit ANYWHERE on the capsule collider bottom. For use when player is "hanging" off an ledge on the tip of the collider
        bt = controller.transform.position;
        bt.y -= (controller.height / 2) - controller.radius + groundedDistanceTolerance;

        //Debug.Log((controller.velocity.magnitude * Time.deltaTime));


        Collider[] stuff = Physics.OverlapSphere(bt, controller.radius, LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore);
        //Debug.DrawRay(bt, transform.TransformDirection(Vector3.down) * (controller.radius + .1f), Color.white);

        if (stuff.Length > 0)
        {
            isGrounded = true;
            //Debug.Log("T: spehere overlap");
            return;
        }

        //Debug.Log("F: default");
        isGrounded = false;
        */
    }

    private bool GroundedAtCenter()
    {
        Vector3 bt = controller.transform.position;
        bt.y -= (playerHeight / 2);

        RaycastHit hit;
        if (Physics.Raycast(bt, Vector3.down, out hit, groundedDistanceTolerance))
            return true;
        return false;
    }
    private bool GroundedAnywhere()
    {
        //return false; //TODO: Make this less bad. Please this is painful whyyyyy

        Vector3 bt = controller.transform.position;
        bt.y -= (controller.height / 2) - controller.radius + groundedDistanceTolerance;
        
        Collider[] stuff = Physics.OverlapSphere(bt, controller.radius, LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Ignore);
        if (stuff.Length > 0) return true;
        return false;
    }
    public bool NearGroundVector(ref Vector3 vector)
    {
        RaycastHit hit;

        Vector3 bt = controller.transform.position;
        bt.y -= (playerHeight / 2);

        if (Physics.Raycast(bt, Vector3.down, out hit, Mathf.Infinity))
        {
            if (!movementOverrideEvent)
            {

                //AppendDesiredMovement(new Vector3(0, -2 /* hit.distance*/ * Time.deltaTime - predictiveOffset, 0));// any reason this is on the heap? : Destroyed via OOS nearly instantly. -Jared
                //isGrounded = true;

            }
            vector = hit.normal;
            //Debug.Log("raycast got a true");
            return true;
        }
        return false;
    }
    public bool NearGroundVector()
    {
        RaycastHit hit;

        Vector3 bt = controller.transform.position;
        bt.y -= (playerHeight / 2);

        if (Physics.Raycast(bt, Vector3.down, out hit, groundedDistanceTolerance))
        {
            return true;
        }
        return false;
    }

}
