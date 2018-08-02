using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ship controll

public class ShipConroller : MonoBehaviour
{
    private MovementEngine engine;
    private Ship ship;
    private float shipSpeed;


    void Start()
    {
        ship = GetComponent<Ship>();
        engine = GetComponent<MovementEngine>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            shipSpeed += ship.ShipAcceleration;
            engine.SetVelocity(shipSpeed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            shipSpeed -= ship.ShipAcceleration;
            engine.SetVelocity(shipSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, 0f, ship.ShipRotationSpeed);
            engine.SetRotation(newRotation);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, 0f, -ship.ShipRotationSpeed);
            engine.SetRotation(newRotation);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ship.SpawnPlane();
        }
    }
}
