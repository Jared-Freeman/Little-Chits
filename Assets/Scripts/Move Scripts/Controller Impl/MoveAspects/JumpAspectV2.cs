using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAspectV2 : MoveAspect
{
    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private float fall = 0f;
    [SerializeField]
    private float jump = 0f;
    [SerializeField]
    private bool canJump;
    private float jumpBuffer = 0f;

    public override void DoUpdate()
    {
        if (jumpBuffer > 0) jumpBuffer -= Time.deltaTime;

        base.DoUpdate();
        canJump = moveSystem.groundedAnywhere;
        isJumping = Input.GetButtonDown("Jump");

        if (isJumping && !canJump) jumpBuffer = .1f;

        if (moveSystem.IsGrounded())
        {
            fall = 0f;
            jump = 0f;
        }
        else
        {
            fall += moveSystem.gravity * moveSystem.gravityScale * Time.deltaTime;
        }

        if ((isJumping || jumpBuffer > 0f) && canJump)
        {
            jump = 5f;
        }

        jump += fall * Time.deltaTime;
        moveSystem.AppendDesiredMovement(new Vector3(0, jump * Time.deltaTime, 0));
    }


}
