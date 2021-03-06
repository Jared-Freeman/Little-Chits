using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Name: Mouselook
// Author: Jared Freeman
// Desc: 
// Simple gameobject manipulator that pitches attached gameobject and yaws transform of player
public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity;
    public float lowerLookBoundary;
    public float upperLookBoundary;

    public Transform playerBody;
    float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, lowerLookBoundary, upperLookBoundary);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);

    }
}
