using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* WalkAspect:
 * Allows player to control their character via directional keys.
 * Movement and sprint can be remotely enabled and disabled.
 */

public class WalkAspect : MoveAspect
{
    //constructor(s)
    public WalkAspect() : base()
    {
        movementEnabled = true;
        sprintEnabled = true;
        canBeInterrupted = true;
    }

    #region members
    //public members
    [Header("Walk Aspect Settings")]
    public bool sprintEnabled;
    public float strafeSpeed = 2f; //could be rewritted as backwardScale * walkSpeed maybe
    public float walkSpeed = 4f;
    public float sprintSpeed = 6f;
    [Range(0, 1)]
    public float backwardScale = .5f;

    //private member
    private bool movementEnabled;
    #endregion

    #region trivial methods
    //methods
    public void EnableMovement() { movementEnabled = true; }
    public void DisableMovement() { movementEnabled = false; }
    public void ToggleMovement() { movementEnabled = !movementEnabled; }
    public bool CanMove() { return movementEnabled;  }

    public void EnableSprint() { sprintEnabled = true; }
    public void DisableSprint() { sprintEnabled = false; }
    public void ToggleSprint() { sprintEnabled = !sprintEnabled; }
    public bool CanSprint() { return sprintEnabled; }
    #endregion
    
    //Complex methods
    public override void DoUpdate( )
    {
        //...any nonmovement things can be put here. probably not needed but whatever!
        if (CanMove()) DoMove(moveSystem);
    }

    private void DoMove(MoveSystem moveSystem)
    {
        float x = Input.GetAxis("Horizontal");
        float z = Mathf.Clamp(Input.GetAxis("Vertical"), -backwardScale, 1);

        bool isSprinting = false;
        if (sprintEnabled) isSprinting = Input.GetAxis("Sprint") > 0f;


        Vector3 move = Vector3.zero;

        if (isSprinting && z > 0) //cannot sprint backwards
        {

            move = strafeSpeed * transform.right * x + sprintSpeed * transform.forward * z;

            if (move.sqrMagnitude > sprintSpeed * sprintSpeed)
            {
                move.Normalize();
                move *= sprintSpeed;
            }
        }
        else
        {
            move = strafeSpeed * transform.right * x + walkSpeed * transform.forward * z;

            /*
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
            */
        }

        

        moveSystem.AppendDesiredMovement(move * Time.deltaTime);
    }

}
