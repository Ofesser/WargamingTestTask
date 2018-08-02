using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Отвечает за управление кораблём

public class ShipConroller : MonoBehaviour {

    [SerializeField]
    private float acceleration = 0.01f;
    [SerializeField]
    private float rotationSpeed = 1f;

    private Motor motor;
    private float shipSpeed;


    void Start()
    {
        motor = GetComponent<Motor>();
    }
	
    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            shipSpeed += acceleration;
            motor.SetVelocity(shipSpeed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            shipSpeed -= acceleration;
            motor.SetVelocity(shipSpeed);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Quaternion newRotation = transform.rotation * Quaternion.Euler(0f,0f,rotationSpeed);
            motor.SetRotation(newRotation);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Quaternion newRotation = transform.rotation * Quaternion.Euler(0f, 0f, -rotationSpeed);
            motor.SetRotation(newRotation);
        }
    }
}
