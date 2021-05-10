using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookAspect : MoveAspect
{

    #region members
    //public members
    [Header("MouseLook Aspect Settings")]
    public bool smoothMouse;
    public bool clampSmoothUpper;
    public static float mouseSensitivity = 75f;
    [Range(-90f, 0f)]
    public float lowerLookBoundary;
    [Range(0f, 90f)]
    public float upperLookBoundary;
    [Range(1, 6)]
    [Tooltip("Number of frames we average. DO NOT SET BELOW 1")]
    public int smoothFrames;

    //private members
    private float xRotation = 0f;
    private Queue<float> axisStackX = new Queue<float>();
    private Queue<float> axisStackY = new Queue<float>();
    #endregion

    public void SetMouseSensitivity(float sens)
    {
        mouseSensitivity = sens;
    }

    public override void InitializeMoveAspect()
    {
        if(aspectEnabled) Cursor.lockState = CursorLockMode.Locked;
        if (smoothFrames < 1) smoothFrames = 1;
    }
    
    //modifying enabled in inspector during runtime won't call these
    public override void EnableAspect()
    {
        base.EnableAspect();
        Cursor.lockState = CursorLockMode.Locked;
    }
    public override void DisableAspect()
    {
        base.EnableAspect();
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public override void DoUpdate( )
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //Debug.Log("X/Y mouse: " + mouseX + " / " + mouseY);

        if(smoothMouse)
        {
            PushToAxisStack(mouseX, mouseY);
            
            xRotation -= YAverage();
            xRotation = Mathf.Clamp(xRotation, lowerLookBoundary, upperLookBoundary);
            moveSystem.playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            moveSystem.transform.Rotate(Vector3.up * XAverage());
        }
        else
        {
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, lowerLookBoundary, upperLookBoundary);
            moveSystem.playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            moveSystem.transform.Rotate(Vector3.up * mouseX);
        }


        //Quaternion prevRot = moveSystem.transform.rotation;

        //moveSystem.transform.rotation = Quaternion.Lerp(moveSystem.transform.rotation, prevRot, .5f);
    }

    private float YAverage()
    {
        float avg = 0f;

        /*
        float[] ys = axisStackY.ToArray();
        for (int i = 0; i < axisStackY.Count; i++)
        {
            avg += ys[i];
            Debug.Log("y: " + ys[i]);
        }
        avg /= axisStackY.Count;
        */

        foreach(float value in axisStackY)
        {
            avg += value;
            //Debug.Log("y: " + value);
        }
        avg /= axisStackY.Count;
        if (clampSmoothUpper)
        {
            float bound = Mathf.Abs(axisStackY.ToArray()[0]);
            avg = Mathf.Clamp(avg, -bound, bound);
        }
        
        return avg;
    }

    private float XAverage()
    {
        float avg = 0f;

        foreach (float value in axisStackX)
        {
            avg += value;
            //Debug.Log("x: " + value);
        }
        avg /= axisStackX.Count;
        if (clampSmoothUpper)
        {
            float bound = Mathf.Abs(axisStackX.ToArray()[0]);
            avg = Mathf.Clamp(avg, -bound, bound);
        }

        return avg;
    }

    //safety method!
    void PushToAxisStack(float x, float y)
    {
        //can edit during runtime!
        while (axisStackX.Count >= smoothFrames)
        {
            axisStackX.Dequeue();
        }
        while (axisStackY.Count >= smoothFrames)
        {
            axisStackY.Dequeue();
        }

        axisStackX.Enqueue(x);
        axisStackY.Enqueue(y);
        
        //Debug.Log("stackcount x: " + axisStackX.Count + ", y: " + axisStackY.Count);
    }
}
