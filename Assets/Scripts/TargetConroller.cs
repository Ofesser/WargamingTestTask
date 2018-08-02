using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Target behaviour

public class TargetConroller : MonoBehaviour {


    [SerializeField]
    private GameObject futureTargetPrefab; // Future-target prefab

    private float targetSpeed;
    private float futureTargetPeriod = 30f; // Coeff of movement future-target from main target
	private GameObject futureTarget;
    private Transform planeTransform;
    private bool isLanding; // Indicator of landing start
    private float landingTime; // Turning time before landing
    private float landingTimer;
    private Vector3 lastTargetPosition; // Last position before landing start

    public GameObject FutureTarget 
    {
        get{return futureTarget;}
    } 

    public Transform PlaneTransform 
    {
        get {return planeTransform;}
        set { planeTransform = value;}
    } 

	private void Start()
	{
        futureTarget = Instantiate(futureTargetPrefab,transform.parent);
        planeTransform.GetComponent<PlaneController>().LandingSignal += StartLanding;
        planeTransform.GetComponent<PlaneController>().LandingDone += DestroyTargets;
	}

	private void FixedUpdate()
	{
        if (!isLanding)
        {
            transform.localPosition = Quaternion.Euler(0f, 0f, -targetSpeed) * transform.localPosition;
            lastTargetPosition = transform.localPosition;
            // The distance to future-target depends on distance between plane and main target
            futureTargetPeriod = Vector3.Magnitude(transform.position - planeTransform.transform.position) * 10f;

            futureTarget.transform.localPosition = Quaternion.Euler(0f, 0f, -targetSpeed * futureTargetPeriod) * transform.localPosition;
        }
        else
        {
            // After the signal of the approach, the plane flies to infinity for a certain time
            // To avoid the situation of inability to land due the low speed of rotation
            landingTimer += Time.fixedDeltaTime;
            if (landingTimer < landingTime)
            {
                transform.localPosition = lastTargetPosition * 1000f;
            }
            else
            {
                // Heading to the ship
                transform.localPosition = Vector3.zero;
            }

            futureTarget.transform.localPosition = transform.localPosition;
        }

	}

    public void SetSpeed(float newSpeed)
    {
        targetSpeed = newSpeed;
    }

    public void StartLanding(float newLandingTime)
    {
        landingTime = newLandingTime;
		isLanding = true;
    }

    private void DestroyTargets(GameObject obj)
    {
        Destroy(futureTarget);
        Destroy(gameObject);
    }
}
