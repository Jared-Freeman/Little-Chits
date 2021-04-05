using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerRigidbodyMovement : MonoBehaviour
{

    public Rigidbody rb;
    public CapsuleCollider playerCollider;


    public float walkSpeed = 2.75f;
    public float sprintSpeed = 4;
    public int maxJumps = 1;
    [Range(0f, 3f)]
    public float jumpHeight = 1.3333333f;
    [Range(.05f, .3f)]
    public float jumpCooldown = .05f; //time before player can jump again, upon landing

    private float xInp;
    private float zInp;
    private bool jumpInp;

   
    [SerializeField]
    private bool isGrounded;
    [SerializeField]
    private int numJumps = 0; //updates when player lands or successfully jumps
    private float moveSpeed; //updates based on user input
    [SerializeField]
    private float jumpSpeed;

    private bool groundedLastFrame = false;
    private float curJumpCooldown = .1f;

    private bool canMove = true;

    private void OnCollisionStay(Collision collision)
    {
        if (isGrounded) canMove = true;
        else canMove = false;
    }



    // Start is called before the first frame update
    void Start()
    {
        #region Init Components
        if (GetComponent<Rigidbody>() != null) rb = GetComponent<Rigidbody>();
        else rb = gameObject.AddComponent<Rigidbody>();

        //TODO:
        //change constraints rb


        if (GetComponent<CapsuleCollider>() != null) playerCollider = GetComponent<CapsuleCollider>();
        else playerCollider = gameObject.AddComponent<CapsuleCollider>();
        #endregion

        #region Init Vars

        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);

        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        #region Update Player Inputs & States
        //Inputs
        xInp = Input.GetAxis("Horizontal");
        zInp = Input.GetAxis("Vertical");

        //States
        if (Input.GetAxis("Sprint") > 0f) moveSpeed = sprintSpeed;
        else moveSpeed = walkSpeed;
        
        UpdateGrounded();

        
        //start cooldown
        if (isGrounded && !groundedLastFrame)
        {
            curJumpCooldown = jumpCooldown;
        }

        if(curJumpCooldown > 0f) curJumpCooldown -= Time.deltaTime;  

        if (isGrounded && (curJumpCooldown <= 0f) && Input.GetKeyDown(KeyCode.Space))
            jumpInp = true;

        if (canMove) DoMove();

        #endregion

        groundedLastFrame = isGrounded;
    }

    // FixedUpdate used for physics calculations (rb)
    private void FixedUpdate()
    {
        //if(canMove) DoFixedMove();




        if (isGrounded && jumpInp && numJumps < maxJumps) { jumpInp = false; DoJump(); }
    }

    #region Helper Methods

    private void DoMove()
    {
        Vector3 moveDirection = transform.TransformDirection(xInp, 0f, zInp);
        moveDirection.Normalize();

        Vector3 planarVel = rb.velocity;
        planarVel.y = 0;
        if (planarVel.sqrMagnitude < moveSpeed * moveSpeed)
            rb.AddForce(1000f * moveDirection * Time.deltaTime, ForceMode.Acceleration);

    }

    private void DoFixedMove()
    {
        Vector3 moveDirection = transform.TransformDirection(xInp, 0f, zInp);

        /*

        RaycastHit hit;

        Vector3 bumpCheck = transform.position;
        bumpCheck.y -= (playerCollider.height / 2) - playerCollider.radius;

        if (Physics.Raycast(
            bumpCheck
            , moveDirection.normalized
            , out hit
            , playerCollider.radius + .05f
            ))
        {

        }
        Debug.DrawRay(bumpCheck
        , moveDirection.normalized * (playerCollider.radius + .05f)
        , Color.red);

        //Physics.SphereCast()

        if (Physics.Raycast(
            transform.position
            , transform.TransformDirection(Vector3.down)
            , out hit
            , playerCollider.height/2 + .3f //double what it needs to be
            ))
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, hit.normal);

            //moveDirection += Physics.gravity * Time.deltaTime;

            if (moveDirection.x * moveDirection.x + moveDirection.z * moveDirection.z
                > moveDirection.y * moveDirection.y)
                rb.MovePosition(transform.position + Time.deltaTime * moveSpeed * moveDirection
                    );
        }
        else
            rb.MovePosition(transform.position + Time.deltaTime * moveSpeed * moveDirection);
        Debug.DrawRay(transform.position
        , transform.TransformDirection(Vector3.down) * (playerCollider.height / 2 + .3f)
        , Color.yellow);
        */


       
         /*
       if(isGrounded && rb.velocity.sqrMagnitude < moveSpeed * moveSpeed)
            rb.AddForce(1000f * Time.deltaTime * moveSpeed * moveDirection.normalized
                    , ForceMode.Acceleration
                );
       else
        {
            Vector3 horizVel = rb.velocity;
            horizVel.y = 0;
            if(horizVel.sqrMagnitude < moveSpeed * moveSpeed)
            {
                rb.AddForce(1000f * Time.deltaTime * moveSpeed * moveDirection.normalized
                    , ForceMode.Acceleration
                    );
            }
        }*/

        Debug.DrawRay(transform.position
        , moveDirection.normalized, Color.blue);
    }
    

    private void UpdateGrounded()
    {
        if (CheckGrounded())
        {   //grounded

            //update stuff
            isGrounded = true;
            numJumps = 0;

        }
        else
        {   //not grounded

            isGrounded = false;
            //update stuff

        }
    }

    private bool CheckGrounded()
    {
        //immediately exit if we are going upward
        //if (rb.velocity.y > 0) return false;


        //check if we are approx. on the ground
        /*
        float groundDistMax = (playerCollider.radius * 0.70710678118f) + .01f; //r * sqrt2/2 + .01f;
        RaycastHit hit;
        Vector3 desiredGroundPos = playerCollider.transform.position;
        desiredGroundPos.y -= ((playerCollider.height / 2f)) - .01f;

        
        Debug.DrawRay(desiredGroundPos
            , transform.TransformDirection(Vector3.down) * groundDistMax, Color.blue);
        if (Physics.Raycast(
            desiredGroundPos
            , transform.TransformDirection(Vector3.down)
            , out hit
            , groundDistMax
            ))
        {
            return true;
        }
        */


        Vector3 bt = playerCollider.transform.position;
        bt.y -= (playerCollider.height/2) - playerCollider.radius;

        Collider[] stuff = Physics.OverlapSphere(bt, playerCollider.radius + .1f, Physics.AllLayers, QueryTriggerInteraction.Ignore); ;
        Debug.DrawRay(bt
           , transform.TransformDirection(Vector3.down) * (playerCollider.radius + .1f), Color.white);


        if ( stuff.Length > 0 )
        {
            return true;
        }
       

        return false;
    }

    private void DoJump()
    {
        numJumps++;
        Vector3 jumpVector = Vector3.up;
        jumpVector *= jumpSpeed;
        rb.AddForce(jumpVector, ForceMode.VelocityChange);
    }

    #endregion
}
