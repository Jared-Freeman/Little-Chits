using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadbobAspect : MoveAspect
{

    #region public members
    [Header("[ADD THESE] Refs")]
    public DevStepMaker stepScript;

    public AnimationCurve groundHitAnimCurve;
    public AnimationCurve walkingAnimCurve;
    //we use same curve for all of airtime (rising and falling) for convenience
    public AnimationCurve airCurve;

    [Header("Headbob Aspect Settings")]
    [Tooltip("Camera tilts when moving sideways")]
    public bool tiltEnabled = true;

    [Tooltip("Hitting the ground produces a downward bob offset")]
    public bool groundHitBobEnabled = true;

    //TODO: average human stride is 2.5ft = ~.76m = .76units per footstep. Use velocity to footstep and bob at appropriate timing
    [Tooltip("Factors in velocity into bob cycle")]
    public bool experimentalMovementBob = false;

    [Tooltip("Use Headbob step sounds")]
    public bool stepSoundsOnBob = true;
    
    [Tooltip("How low can the camera sink upon hitting the ground? Used if groundHitBobEnabled == true")]
    public float airtimeMinHeight = -.4f;

    [Range(0f, 1f)]
    [Tooltip("How intense the headbob is up and down")]
    public float bobIntensity = .8f; //can be any value at this point smh
    
    [Range(0f, 1f)]
    [Tooltip("Intensity of side tilt on camera")]
    public float tiltIntensity = .35f;
    #endregion

    #region private members
    private float lerpInterpolant = .6f;
    private float bobCycle = 0f; //span length of 0-2pi over and over...
    private float airtimeOffsetDesired = 0f;
    private float airtimeOffsetActual = 0f;
    private float airtimeLastFrameVerticalSpeed = 0f;
    private float airtimeOffsetCoefficient;

    private readonly float maxTiltAngle = 2f;
    private float desiredAngle = 0f;
    private float curAngle = 0f;

    private float vertBobAnimCurTime = 0f;
    #endregion
    
    public override void DoUpdate( )
    {
        lerpInterpolant = (1 - Mathf.Pow(.1f, Time.deltaTime));

        if (tiltEnabled) DoTilt(moveSystem);

        if (bobIntensity <= 0f) return;

        airtimeOffsetCoefficient = Mathf.Abs(airtimeMinHeight / moveSystem.maxFallSpeed);
        UpdateAirtimeActual();
        
        bool isGrounded = moveSystem.IsGrounded();
        bool isMoving = moveSystem.IsMoving();

        if (!groundHitBobEnabled) ResetAirtimeOffset(); //nasty way of doing this lol

        //player is moving and not midair
        if (isMoving && isGrounded)
        {
            //Debug.Log(moveSystem.GetVelocity().magnitude);

            if(experimentalMovementBob)
                bobCycle = (bobCycle + Time.deltaTime * (7.5f + moveSystem.GetVelocity().magnitude) * 1.5f );
            else
                bobCycle = (bobCycle + Time.deltaTime * 12f); //1 sin/cos wave is 2pi

            if (stepSoundsOnBob)
            {
                if(bobCycle > (Mathf.PI * 2))
                {
                    stepScript.DoStep();
                }
            }

            bobCycle %= (Mathf.PI * 2);

            Vector3 bobVector;

            if (experimentalMovementBob)
                bobVector = GetBobVector(bobCycle, 0f, .1f * bobIntensity * (moveSystem.GetVelocity().magnitude), moveSystem.GetCameraOriginalPosition()); //test vals
            else
                bobVector = GetBobVector(bobCycle, 0f, .085f * bobIntensity, moveSystem.GetCameraOriginalPosition()); //test vals


            bobVector.y += airtimeOffsetActual;
            moveSystem.playerCamera.transform.localPosition = Vector3.Lerp(moveSystem.playerCamera.transform.localPosition, bobVector, lerpInterpolant);
        }
        //player is at rest. Occasionally this is a false positive for a frame or so
        else if (isGrounded)
        {

            bobCycle = (bobCycle + Time.deltaTime * 2f) % (Mathf.PI * 2); //1 sin/cos wave is 2pi

            Vector3 bobVector = GetBobVector(bobCycle, 0f, .085f * bobIntensity * .45f, moveSystem.GetCameraOriginalPosition()); //scaled down moving bob
            bobVector.y += airtimeOffsetActual;
            moveSystem.playerCamera.transform.localPosition = Vector3.Lerp(moveSystem.playerCamera.transform.localPosition, bobVector, lerpInterpolant);
            

            /*
            bobCycle += 3f * Time.deltaTime * (bobCycle > 0 ? -1 : 1);
            if (Mathf.Abs(bobCycle) < .01f) bobCycle = 0f;

            Vector3 bobVector = moveSystem.GetCameraOriginalPosition();
            bobVector.y += airtimeOffsetActual;

            //a more relaxed interpolant
            moveSystem.playerCamera.transform.localPosition = Vector3.Lerp(moveSystem.playerCamera.transform.localPosition, bobVector, lerpInterpolant * .1f);
            */
        }
        //player in air
        else
        {
            if (bobCycle < (Mathf.PI * .5f))
            {
                bobCycle = BobLerp(bobCycle, Mathf.PI * .5f, lerpInterpolant);
                bobCycle = Mathf.Clamp(bobCycle, 0f, Mathf.PI * .5f);
            }
            else if (bobCycle < (Mathf.PI))
            {
                bobCycle = BobLerp(bobCycle, Mathf.PI * 1f, lerpInterpolant);
                bobCycle = Mathf.Clamp(bobCycle, Mathf.PI * .5f, Mathf.PI * 1f);
            }
            else if (bobCycle < (Mathf.PI * 1.5f))
            {
                bobCycle = BobLerp(bobCycle, Mathf.PI * 1.5f, lerpInterpolant);
                bobCycle = Mathf.Clamp(bobCycle, Mathf.PI * 1f, Mathf.PI * 1.5f);
            }
            else if(bobCycle < (Mathf.PI * 1.5f))
            {
                bobCycle = BobLerp(bobCycle, Mathf.PI * 2f, lerpInterpolant);
                bobCycle = Mathf.Clamp(bobCycle, Mathf.PI * 1.5f, Mathf.PI * 2f);
            }
            else { } //we are at a zero. do nothing


            airtimeLastFrameVerticalSpeed = moveSystem.controller.velocity.y; //we record vertical speed here to determine bonk intensity

            ResetAirtimeOffset();
            
            Vector3 bobVector = GetBobVector(bobCycle, 0f, .085f * bobIntensity * .45f, moveSystem.GetCameraOriginalPosition()); //scaled down moving bob
            bobVector.y += airtimeOffsetActual;

            moveSystem.playerCamera.transform.localPosition = Vector3.Lerp(moveSystem.playerCamera.transform.localPosition, bobVector, lerpInterpolant * .05f);

        }

        //record is airtime last frame at end of update, reduce airtimeLastFrameVerticalSpeed over time to settle to 0
        if (isGrounded)
        {
            if (airtimeLastFrameVerticalSpeed < 0)
            {
                float speedReducer = 18.0f; // TODO get a name for this boi
                airtimeLastFrameVerticalSpeed += speedReducer * Time.deltaTime;
                Mathf.Clamp(airtimeLastFrameVerticalSpeed, Mathf.NegativeInfinity, 0);
            }
            else airtimeLastFrameVerticalSpeed = 0;
        }

    }//end DoUpdate()

    public override void OnGroundHit()
    {
        base.OnGroundHit();
        stepScript.DoFall();
        vertBobAnimCurTime = 0f;
    }

    public override void OnAirtimeStart()
    {
        base.OnAirtimeStart();
        vertBobAnimCurTime = 0f;
    }

    private Vector3 GetBobVector(float curCycle, float bobIntensityX, float bobIntensityY, Vector3 camOriginalPosition)
    {
        Vector3 bobVector = new Vector3(Mathf.Cos(curCycle) * bobIntensityX
            , Mathf.Sin(curCycle) * bobIntensityY
            , camOriginalPosition.z);

        bobVector += camOriginalPosition;

        return bobVector;
    }

    private void UpdateAirtimeActual()
    {

        if (airtimeOffsetDesired < airtimeLastFrameVerticalSpeed)
        {
            airtimeOffsetDesired += (airtimeOffsetCoefficient * airtimeLastFrameVerticalSpeed - airtimeOffsetDesired) * Time.deltaTime * 1.5f;
            if (Mathf.Abs(airtimeOffsetDesired) < .01f) airtimeOffsetDesired = 0f;
        }
        else if (airtimeOffsetDesired > airtimeLastFrameVerticalSpeed)
        {
            airtimeOffsetDesired += airtimeLastFrameVerticalSpeed * Time.deltaTime;
        }
        airtimeOffsetDesired = Mathf.Clamp(airtimeOffsetDesired, airtimeMinHeight, 0f);


        float airtimeOffsetDiff = (airtimeOffsetDesired - airtimeOffsetActual);
        if (Mathf.Abs(airtimeOffsetDiff) < .0001 || airtimeOffsetActual > 0) airtimeOffsetActual = airtimeOffsetDesired;
        else
        {
            airtimeOffsetActual = airtimeOffsetDiff * .9f;
        }
    }

    private void ResetAirtimeOffset()
    {
        airtimeOffsetDesired = 0f;
        airtimeOffsetActual = 0f;
    }

    //TODO: fix tilt jitter on object collide (random falses to isMoving..., random loss of grounded...)
    //TODO: double dog fix tilt! it jittery as hell when pushin :(
    //TODO: lol maybe lerp between animation curve outputs or do SOME KIND OF BLENDING JAUSUS
    //IDEA: define a buffer of frames (some variable size) and blend from one function to the other. Otherwise just some time interval t.
    //      Choose an interpolant that favors predecessor and morphs to favor successor animation... 

    // DoTilt: Tilts the cam a bit depending on strafe, movement, hitting the ground, and flying in air
    private void DoTilt(MoveSystem moveSystem)
    {
        float axis = Input.GetAxis("Horizontal");
        float rate = 10f; //Rate of change for angle

        desiredAngle = (axis * maxTiltAngle * tiltIntensity);

        if (curAngle < desiredAngle)
        {
            curAngle += rate * Time.deltaTime;
            if (curAngle > desiredAngle) curAngle = desiredAngle;
        }
        else if (curAngle > desiredAngle)
        {
            curAngle -= rate * Time.deltaTime;
            if (curAngle < desiredAngle) curAngle = desiredAngle;
        }

        float offset = 0f;

        if ( !moveSystem.IsGrounded()/* && moveSystem.IsLegalAirtime()*/) // IsLegalAirtime() used for jitter correction (multi-frame "false positives"). Won't catch the first one if player was grounded a long time, only long osc's.
        {
            //TODO: play flying ("lift") bob
            //we can use the same vertBobAnimCurTime var in here since it will be reset upon hitting ground

            //going up
            if(moveSystem.controller.velocity.y > 0)
            {
                if (vertBobAnimCurTime > .5f) vertBobAnimCurTime = .5f; //just in case
                if (vertBobAnimCurTime < .5f)
                {
                    vertBobAnimCurTime += Time.deltaTime * 2;
                    vertBobAnimCurTime = Mathf.Clamp(vertBobAnimCurTime, 0f, .5f);
                }
                //do offset thingy
            }
            //falling down
            else
            {
                if (vertBobAnimCurTime < .5f) vertBobAnimCurTime += 1f - vertBobAnimCurTime; //get spot reflected across x=.5 on graph
                if (vertBobAnimCurTime < 1f)
                {
                    vertBobAnimCurTime += Time.deltaTime * 2;
                    vertBobAnimCurTime = Mathf.Clamp(vertBobAnimCurTime, .5f, 1f);
                }
            }

            offset += -8f * airCurve.Evaluate(vertBobAnimCurTime);
        }
        else
        {
            //play the fell bob if it's there
            if(vertBobAnimCurTime <= 1f)
            {
                offset += (bobIntensity + .6f) * -28f * groundHitAnimCurve.Evaluate(vertBobAnimCurTime);
                vertBobAnimCurTime += Time.deltaTime * 1.4f; //Coeff is the time for full cycle
            }
            //...else play the bob based on player run
            else
            {

            }
        }

        moveSystem.playerCamera.transform.localRotation = Quaternion.Lerp(moveSystem.playerCamera.transform.localRotation
            , Quaternion.Euler(moveSystem.playerCamera.transform.localRotation.eulerAngles.x + offset
                  , moveSystem.playerCamera.transform.localRotation.eulerAngles.y
                  , -1f * curAngle)
            , .6f);
        
    }

    float BobLerp(float start, float end, float t)
    {
        return start * (1 - t) + end * t;
    }

    
}
