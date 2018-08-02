using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Plane behaviour

public class PlaneController : MonoBehaviour {

    private float minSpeed;
    private float maxSpeed;
	private float maxRotationSpeed;
    private float planeRadius;
    private float maxFlightTime; //Max time before the plane starts approaching
    private float flightTimer;
    private bool isLanding;
    private TargetConroller target;
    private float middleSpeed;
    private bool contactedWithTarget;
    private MovementEngine engine;

	public Action<float> LandingSignal; // Signal of starting approaching
    public Action<GameObject> LandingDone; // Signal of finishing landing

    public TargetConroller Target
    {
        get { return target; }
        set { target = value; }
    }

    void Start()
    {
        engine = GetComponent<MovementEngine>();
        middleSpeed = Utils.GetMiddleValue(minSpeed, maxSpeed);
        float angularVelocity = Utils.CalculateAngVelByLinVel(middleSpeed, planeRadius);
        target.GetComponent<TargetConroller>().SetSpeed(angularVelocity);
    }
	
	void Update () {
        engine.SetVelocity(CalculateVelocity());
        engine.SetRotation(CalculateRotation());

        if (!isLanding)
        {
            flightTimer += Time.deltaTime;
            if (flightTimer > maxFlightTime)
            {
                float landingTime = Utils.FindReturnTime(planeRadius, Utils.GetMiddleValue(minSpeed, maxSpeed));
                LandingSignal(landingTime);
                isLanding = true;
            }
        }
	}

    public void SetPlaneParameters(float newMinSpeed, float newMaxSpeed, float newmaxRotationSpeed,
                                   float newplaneRadius, float newMaxFlightTime)
    {
        minSpeed = newMinSpeed;
        maxSpeed = newMaxSpeed;
        maxRotationSpeed = newmaxRotationSpeed;
        planeRadius = newplaneRadius;
        maxFlightTime = newMaxFlightTime;
    }

    private float CalculateVelocity()
    {
        // Plane always heading to the future-target
        Vector3 direction = target.FutureTarget.transform.position - transform.position;
        float distance = direction.magnitude;
        float planeCalculatedVelocity = maxSpeed - middleSpeed / (1f + distance);
        float planeClampedVelocity = Mathf.Clamp(planeCalculatedVelocity, minSpeed, maxSpeed);

        // If the plane is directing torwards away from targets it should reduce the speed
        Vector3 targetDirection = target.transform.position - transform.position;

        if (Vector3.Angle(transform.up, direction) < 30f && Vector3.Angle(transform.up, targetDirection) < 30f)
        {
            planeClampedVelocity = Mathf.Lerp(planeClampedVelocity, maxSpeed, 0.1f);
        }
        else
        {
            planeClampedVelocity = Mathf.Lerp(planeClampedVelocity, minSpeed, 0.1f);
        }

        // The plane is slowing down when contacts with target
        if (contactedWithTarget)
        {
            planeClampedVelocity = Mathf.Lerp(planeClampedVelocity, minSpeed, 0.1f);
        }

        return planeClampedVelocity;
    }

    private Quaternion CalculateRotation()
    {
        Vector3 direction = target.FutureTarget.transform.position - transform.position;
        Quaternion toRotation = Quaternion.FromToRotation(Vector3.up, direction);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, toRotation, maxRotationSpeed);

        return newRotation;
    }

    void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.gameObject == target.gameObject)
        {
            contactedWithTarget = true;
        }

        // Landing hanlder
        if (triggerCollider.gameObject == target.gameObject && triggerCollider.transform.localPosition == Vector3.zero)
        {
            LandingDone(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider triggerCollider)
	{
        if (triggerCollider.gameObject == target.gameObject)
        {
            contactedWithTarget = false;
        }
	}
}
