using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (CharacterController))]

public class PlayerMovementOld : MonoBehaviour
{

    public CharacterController controller;
    private float gravity = -9.81f;
    public float gravityScale = 2f;
    private float maxFallSpeed = 24f;
    public float jumpHeight = 2f; //allows alteration of jump height before runtime
    private float jumpSpeed;
    private float vertSpeed = 0;
    public float strafeSpeed = .5f * 100;
    public float walkSpeed = 1.5f * 100;
    public float backwardScale = .4f;
    private float groundedDistanceTolerance = .2f;
    private Vector3 heightDiv2;

    public float getMaxFallSpeed()
    {
        return maxFallSpeed;
    }

    // Asissts smooth movement over sloped surfaces.
    // Will "snap" player to ground if they are groundedDistanceTolerance away. 
    // Otherwise will not move player.
    public bool IsGrounded()
    {
        //if we have vertical speed we are NOT grounded! Should be first check (jumps will not work otherwise)!
        if (vertSpeed > 0) return false;
        if (controller.isGrounded) return true;
        
        Vector3 bottomOfPlayer = controller.transform.position - heightDiv2;

        RaycastHit hit;
        if (Physics.Raycast(bottomOfPlayer, Vector3.down, out hit, groundedDistanceTolerance))
        {
            controller.Move(new Vector3(0, -1 * hit.distance, 0));
            return true;
        }

        return false;
    }



    private void Start()
    {
        if (GetComponentInParent<CharacterController>() != null)
            controller = GetComponentInParent<CharacterController>();

        jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * gravity * gravityScale);

        vertSpeed = 0;

        heightDiv2 = new Vector3(0, controller.height / 2, 0);
    }



    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        if (z < 0) z *= backwardScale; //scale z

        Vector3 move = strafeSpeed * transform.right * x + walkSpeed * transform.forward * z;

        //walkSpeed magnitude should not be exceeded by strafe + walk vector sum magnitude
        if (move.sqrMagnitude > walkSpeed * walkSpeed)
        {
            move.Normalize();
            move *= walkSpeed;
            if (z < 0) //detect backwards movement, scale appropriately
            {
                move *= backwardScale;
            }
        }
        
        //simple jump
        if (IsGrounded())
        {
            vertSpeed = 0;
            if (Input.GetButtonDown("Jump"))
            {
                vertSpeed = jumpSpeed;
            }
        }
        else
        {
            vertSpeed += gravityScale * gravity * Time.deltaTime; //adjust vertSpeed
            vertSpeed = Mathf.Clamp(vertSpeed, -1f * maxFallSpeed, Mathf.Infinity); //we only bound on lower end. We dont care about max upward speed. We COULD use pos maxFallSpeed tho
        }
        
        move.y = vertSpeed;
        controller.Move(move * Time.deltaTime);
    }




}
