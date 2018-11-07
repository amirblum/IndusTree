using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    //Public variable to store a reference to the player game object
    public GameObject Midpoint;
    public float Xoffset;
    public float Zoffset;



    public Vector3 offset;         //Private variable to store the offset distance between the player and camera

    private Vector3 CalculateCameraPositionAccordingToMidpoint ()
    {
        
        Vector3 MidpointPosition = Midpoint.transform.position;
        Vector3 newPosition = new Vector3((Midpoint.transform.position.x) / 2 - Xoffset, transform.position.y, Midpoint.transform.position.z / 2 - Zoffset);
        return newPosition;
    }

    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        

    }

    // LateUpdate is called after Update each frame

    private void Update()
    {
        transform.position = CalculateCameraPositionAccordingToMidpoint();
    }
}