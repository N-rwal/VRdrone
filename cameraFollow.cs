using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera
    public float distanceFromCamera = 2.0f; // Distance from the camera

    void Update()
    {
        // Set the position in front of the camera
        transform.position = cameraTransform.position + cameraTransform.forward * distanceFromCamera;

        // Object should always follow the camera
        transform.rotation = cameraTransform.rotation;
    }
}