using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Name: JumpAspect
// Author: Jared Freeman
// Desc: 
// Implements jumping in MoveSystem hierarchy. Supports jump cooldowns, fatigue, slide detection, and a few other fancy features

public class JumpAspect : MoveAspect
{
    #region ctor
    public JumpAspect() : base()
    {
        useGravity = true;
        canBeInterrupted = true;
    }
    #endregion

    #region members 
    [Header("Jump Aspect Settings")]
    public bool useGravity = true;
    public bool jumpNormal = true;
    [Range(0, 5)]
    public int slideJumps = 3;
    [Range(0f, 2f)]
    public float jumpCooldownExtra = 0f; //the amount of time BEYOND the normal jump cooldown we wait. for use in stamina sim
    [Range(0f, 1f)]
    public float jumpCooldownBase = .35f;
    [Range(.25f, 3f)]
    public float jumpHeight = 1.333333f; //alteration of jump height during runtime is allowed
    [SerializeField]
    private float maxSlideAngle; //deg    
    [SerializeField]
    private bool flag_ignore_ground; //bandaid to fix dumb ground detection scheme I foolishly wrote...


    //private members
    private float jumpHeightDefault;
    private float jumpTimer = 0f;
    private float jumpSpeed;
    private int remainingSlideJumps;

    [SerializeField]
    private float vertSpeed = 0f;

    [SerializeField]
    private Vector3 jumpNormalVector = Vector3.zero;
    private float normalVectorScaler = 0f;

    [SerializeField]
    private bool isSliding;
    #endregion

    #region trivial methods
    public void ResetJumpCooldown() { jumpCooldownExtra = 0f; }

    public void AppendJumpCooldownExtra(float x) { jumpCooldownExtra += x; jumpCooldownExtra = Mathf.Clamp(jumpCooldownExtra, 0f, Mathf.Infinity); } //can append negative values

    public void SetJumpCooldownExtra(float x) {
        if (x >= 0f) jumpCooldownExtra = x;
        else { Debug.LogError("Attempt to set jumpCooldownExtra to negative value!"); }
    }

    public void SetJumpCooldown(float x) {
        if (x - jumpCooldownBase >= 0) jumpCooldownExtra = (x - jumpCooldownBase);
        else { Debug.LogError("Attempt to set jump cooldown below base value!"); }
    }

    public void ResetJumpCooldownTimer() { jumpTimer = 0f; }
    public void DecrementJumpCooldownTimer() { jumpTimer -= 1.5f * Time.deltaTime; jumpTimer = Mathf.Clamp(jumpTimer, 0f, Mathf.Infinity); }

    public float GetJumpHeightDefault() { return jumpHeightDefault; }
    public void ResetJumpHeight() { jumpHeight = jumpHeightDefault; }
    public void SetJumpHeight(float h) { if(jumpHeight >= 0f) jumpHeight = h; else Debug.LogError("Attempt to set jump height to negative value!"); }
    public void AppendJumpHeight(float h) { jumpHeight += h; jumpHeight = Mathf.Clamp(jumpHeight, 0f, Mathf.Infinity); } //can append negative values

    #endregion
    
    public override void InitializeMoveAspect()
    {
        jumpHeightDefault = jumpHeight;
        maxSlideAngle = moveSystem.controller.slopeLimit;
        remainingSlideJumps = slideJumps;
        flag_ignore_ground = false;
    }

    public override void DoUpdate(  )
    {

        Vector3 normal = Vector3.zero;
        moveSystem.NearGroundVector(ref normal);
        isSliding = Vector3.Angle(Vector3.up, normal) > maxSlideAngle - 5; //tolerance needed for some reason

        float jumpCooldownTotal = (jumpCooldownBase + jumpCooldownExtra);
        bool isJumping = Input.GetButtonDown("Jump");

        //Can fix jump losses by adding " || jump_happened_super_recently "
        //But I want something better than a bandaid...
        if (moveSystem.IsGrounded())
        {
            //if(isJumping) Debug.Log("Should be jumping!"); //works from here, still bug appearing

            if (jumpTimer <= jumpCooldownTotal)
            {
                jumpTimer += Time.deltaTime;
                jumpTimer = Mathf.Clamp(jumpTimer, 0f, jumpCooldownTotal);
                moveSystem.AppendDynamicSpeedMultiplier(-1f * (jumpCooldownTotal - jumpTimer));
            }

            ///////////////// BUG IS HERE /////////////////
            /*
            */
            //jumpNormalVector.x = 0;
            //jumpNormalVector.z = 0;
            if (!isSliding && (flag_ignore_ground == false))
            {
                jumpNormalVector.y = 0f;
                vertSpeed = 0f;
                if(moveSystem.IsGrounded()) remainingSlideJumps = slideJumps;
            }
            ///////////////// BUG IS HERE /////////////////

            DoGroundedUpdate(isJumping, jumpCooldownTotal);



            /*

            if (isJumping)
            {
                Vector3 normal = Vector3.zero;
                if (moveSystem.NearGroundVector(ref normal) && Vector3.Angle(Vector3.up, normal) > maxSlideAngle) { DoSlideBehavior(isJumping, jumpCooldownTotal); }
                else if (moveSystem.groundedAtCenter) { DoRegularBehavior(isJumping, jumpCooldownTotal); }
            }
            //no jump
            else
            {
                //slide behavior
                Vector3 normal = Vector3.zero;
                if (!moveSystem.groundedAtCenter && moveSystem.groundedAnywhere || moveSystem.NearGroundVector(ref normal) && Vector3.Angle(Vector3.up, normal) > maxSlideAngle)
                {
                    //we retain vertical speed
                    vertSpeed = Mathf.Clamp(vertSpeed + moveSystem.gravityScale * moveSystem.gravity * Time.deltaTime, -1f * moveSystem.maxFallSpeed, Mathf.Infinity); //adjust vertSpeed
                }
                else
                {
                    jumpNormalVector.y = 0f;
                    vertSpeed = 0f;
                }

            }
          */
        }
        else
        {
            DoAirtimeUpdate();
            /*
            DecrementJumpCooldownTimer();
            vertSpeed = Mathf.Clamp(vertSpeed + moveSystem.gravityScale * moveSystem.gravity * Time.deltaTime, -1f * moveSystem.maxFallSpeed, Mathf.Infinity); //adjust vertSpeed
            */
        }
            
        //Speed adjustments
        if(jumpNormal)
        {
            Vector2 temp = new Vector2(jumpNormalVector.x, jumpNormalVector.z);
            temp *= normalVectorScaler; //MAP THIS TO FALL MAX

            normalVectorScaler = Mathf.Abs(vertSpeed / moveSystem.maxFallSpeed);

            jumpNormalVector.x = temp.x;
            jumpNormalVector.z = temp.y;

            jumpNormalVector.y += moveSystem.gravityScale * moveSystem.gravity * Time.deltaTime;
            jumpNormalVector.y = Mathf.Clamp(jumpNormalVector.y, -1f * moveSystem.maxFallSpeed, Mathf.Infinity);

            moveSystem.AppendDesiredMovement(jumpNormalVector * Time.deltaTime);
        }
        else
        {
            moveSystem.AppendDesiredMovement(new Vector3(0, vertSpeed * Time.deltaTime, 0));
        }
    }

    //generates movement append if we are jumping beyond slide tolerance
    private void DoSlideBehavior(bool isJumping, float jumpCooldownTotal)
    {
        //jump
        if (isJumping && jumpTimer >= jumpCooldownTotal)
        {
            SendMessage("OnJumpEvent", SendMessageOptions.DontRequireReceiver);
            ResetJumpCooldownTimer();
            remainingSlideJumps--;
            if (remainingSlideJumps > 0)
            { 
                if (jumpNormal)
                {
                    jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * moveSystem.gravity * moveSystem.gravityScale);
                    //jumpSpeed -= vertSpeed;
                    normalVectorScaler = 1f;
                    Vector3 normal = Vector3.zero;
                    moveSystem.NearGroundVector(ref normal);
                    normal.Normalize();
                    normal *= jumpSpeed;
                    jumpNormalVector = normal;

                    //moveSystem.AppendDesiredMovement(jumpNormalVector * Time.deltaTime);
                }
                else
                {
                    jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * moveSystem.gravity * moveSystem.gravityScale);
                    vertSpeed = jumpSpeed;
                }
            }
        }
    }
    //generates movement append if we are jumping regularly
    private void DoRegularBehavior(bool isJumping, float jumpCooldownTotal)
    {
        //jump
        if (isJumping && jumpTimer >= jumpCooldownTotal)
        {
            //Debug.Log("JumpEvent!");

            IgnoreGroundForSeconds(.25f); //This MIGHT fix

            SendMessage("OnJumpEvent", SendMessageOptions.DontRequireReceiver);
            ResetJumpCooldownTimer();
            if (jumpNormal)
            {

                //Debug.Log("JumpNormal!");
                jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * moveSystem.gravity * moveSystem.gravityScale);
                jumpNormalVector = Vector3.up; //TODO: project on plane
                jumpNormalVector *= jumpSpeed;

                //moveSystem.AppendDesiredMovement(jumpNormalVector * Time.deltaTime);
            }
            else
            {
                jumpSpeed = Mathf.Sqrt(jumpHeight * -2f * moveSystem.gravity * moveSystem.gravityScale);
                vertSpeed = jumpSpeed;
            }
        }
    }

    private void DoGroundedUpdate(bool isJumping, float jumpCooldownTotal)
    {
        //if(isJumping) Debug.Log("DoGroundedUpdate! sliding: " + isSliding); //sliding isnt causing it
        if (isSliding) //5f is arbitrary tolerance. it was being weird otherwise
        {
            DoAirtimeUpdate(); //uh yeah lol
            DoSlideBehavior(isJumping, jumpCooldownTotal);
        }
        //else behave as usual
        else
        {
            DoRegularBehavior(isJumping, jumpCooldownTotal);
        }
    }
    private void DoAirtimeUpdate()
    {
        DecrementJumpCooldownTimer();
        vertSpeed = Mathf.Clamp(vertSpeed + moveSystem.gravityScale * moveSystem.gravity * Time.deltaTime, -1f * moveSystem.maxFallSpeed, Mathf.Infinity); //adjust vertSpeed
    }

    private void IgnoreGroundForSeconds(float duration)
    {
        StopCoroutine("ContinueIgnoreGroundForSeconds");
        StartCoroutine(ContinueIgnoreGroundForSeconds(duration)); //This MIGHT fix
    }

    private IEnumerator ContinueIgnoreGroundForSeconds(float duration)
    {
        flag_ignore_ground = true;

        float start_time = Time.time;
        float cur_time = Time.time;
        while(Mathf.Abs(cur_time - start_time) < duration)
        {
            cur_time = Time.time;
            yield return null;
        }

        flag_ignore_ground = false;
    }
}
