using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof (CharacterController))]
[RequireComponent (typeof (PlayerMovementOld))]

// ERROR when walking into a wall head bobbing still occurs despite not moving

/* Headbobbing:
 *
 * Note:
 * Code is currently filled with a few magic numbers.
 *
 * Desc:
 * Utilizing information from a charactercontroller and our playermovement script, we simulate
 * simple head bobbing as our camera y follows a sinwave. Our headbobbing function can be effectively changed
 * to any graph, but should loop at some value (0 ideally). Camera also has left/right bob implementation but
 * it was not useful for camera movement. Concepts here could be applied to a weapon bob system.
 */

public class HeadBobbing : MonoBehaviour
{


    //May want to serialize later as these could easily be prefs
    public CharacterController controller;
    public PlayerMovementOld playerMovement;
    public Camera cam;
    public float bobIntensity = .25f; //should be a value between 0-1 // -Kace I like 0.85
    public float lerpInterpolant;
    public float airtimeMinHeight = -.8f;

    private float bobCycle = 0f; //span length of 0-2pi over and over...
    private float airtimeOffsetDesired = 0f;
    private float airtimeOffsetActual = 0f;
    private bool airtimeLastFrame = false;
    private float airtimeLastFrameVerticalSpeed = 0f;
    private bool isMoving;
    private Vector3 camOriginalPosition;
    private float airtimeOffsetCoefficient;



    // Start is called before the first frame update
    void Start()
    {
        lerpInterpolant = .6f;

        if (GetComponent<Camera>() != null)
            cam = GetComponent<Camera>();

        //These components wont be on the camera. Use GetComponentInParent<...>()
        if (GetComponentInParent<CharacterController>() != null)
        {
            controller = GetComponentInParent<CharacterController>();
        }
        if (GetComponentInParent<PlayerMovementOld>() != null)
        {
            playerMovement = GetComponentInParent<PlayerMovementOld>();
        }

        camOriginalPosition = cam.transform.localPosition;

        airtimeOffsetCoefficient = Mathf.Abs(airtimeMinHeight / playerMovement.getMaxFallSpeed());
    }



    // Update is called once per frame
    void Update()
    {
        isMoving = controller.velocity.sqrMagnitude > 0f;
        UpdateAirtimeActual();

        Vector3 newCamPosition = Vector3.zero;
        if (bobIntensity <= 0f) return;
        bool isGrounded = playerMovement.IsGrounded();


        //player is moving and not midair
        if (isMoving && isGrounded)
        {
            bobCycle += Time.deltaTime * 12f;
            bobCycle %= Mathf.PI * 2; //1 sin/cos wave is 2pi

            Vector3 bobVector = GetBobVector(bobCycle, 0f, .085f * bobIntensity); //test vals
            bobVector.y += airtimeOffsetActual;
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, bobVector, lerpInterpolant);
        }
        //player is at rest. Occasionally this is a false positive for a frame or so
        else if (isGrounded)
        {

            if (bobCycle > 0)
            {
                bobCycle -= 3f * Time.deltaTime;
                if (Mathf.Abs(bobCycle) < .01f) bobCycle = 0f;
            }
            else if (bobCycle < 0)
            {
                bobCycle += 3f * Time.deltaTime;
                if (Mathf.Abs(bobCycle) < .01f) bobCycle = 0f;
            }

            Vector3 bobVector = camOriginalPosition;
            bobVector.y += airtimeOffsetActual;

            //a more relaxed interpolant
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, bobVector, lerpInterpolant * .05f);
        }
        //player in air
        else
        {
            airtimeLastFrame = true;
            airtimeLastFrameVerticalSpeed = controller.velocity.y; //we record vertical speed here to determine bonk intensity

            ResetAirtimeOffset();

            Vector3 bobVector = camOriginalPosition;
            bobVector.y += airtimeOffsetActual;

            //a more relaxed interpolant
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, bobVector, lerpInterpolant * .05f);

        }

        //record is airtime last frame at end of update, reduce airtimeLastFrameVerticalSpeed over time to settle to 0
        if (isGrounded)
        {
            airtimeLastFrame = false;
            if (airtimeLastFrameVerticalSpeed < 0)
            {
                airtimeLastFrameVerticalSpeed += 18f * Time.deltaTime;
                Mathf.Clamp(airtimeLastFrameVerticalSpeed, Mathf.NegativeInfinity, 0);
            }
            else airtimeLastFrameVerticalSpeed = 0;
        }
    }



    private Vector3 GetBobVector(float curCycle, float bobIntensityX, float bobIntensityY)
    {
        Vector3 bobVector = new Vector3(Mathf.Cos(curCycle) * bobIntensityX
            , Mathf.Sin(curCycle) * bobIntensityY
            , camOriginalPosition.z);

        bobVector += camOriginalPosition;

        return bobVector;
    }



    private void UpdateAirtimeActual()
    {

        if(airtimeOffsetDesired < airtimeLastFrameVerticalSpeed)
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
}
