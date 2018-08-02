using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Отвечает за движение объектов. В более больших проектах есть смысл сделать этот скрипт родительским
// а для конкретных сущностей описывать их специфику в дочерних скриптах, но в этом проекте этого будет достаточно.

public class Motor : MonoBehaviour {

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