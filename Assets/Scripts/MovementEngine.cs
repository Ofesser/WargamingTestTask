using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Random comment

public class MovementEngine : MonoBehaviour {

    private float velocity = 0f;
    private Quaternion rotation = Quaternion.identity;

    public void SetVelocity(float newVelocity)
    {
        velocity = newVelocity;
    }

    public void SetRotation(Quaternion newRotation)
    {
        rotation = newRotation;
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }

    private void PerformMovement()
    {
        transform.Translate(Vector3.up * velocity * Time.fixedDeltaTime);
    }

    private void PerformRotation()
    {
        transform.rotation = rotation;
    }
}