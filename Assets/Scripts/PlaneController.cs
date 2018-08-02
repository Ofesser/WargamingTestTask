using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Логика движения самолёта

public class PlaneController : MonoBehaviour {

    private float minSpeed;
    private float maxSpeed;
	private float maxRotationSpeed;
    private float planeRadius;

    private float maxFlightTime; // Время после которого самолёт начинает заход на посадку
    private float flightTimer;
    private bool isLanding;

	public Action<float> LandingSignal; // Сигнал начала захода на посадку
    public Action<GameObject> LandingDone; // Сигнал окончания посадки
    private TargetConroller target; 

    private float middleSpeed;
    private bool contactedWithTarget;
    private Motor motor;

    public TargetConroller Target
    {
        get { return target; }
        set { target = value; }
    }

    void Start()
    {
        motor = GetComponent<Motor>();
        middleSpeed = Utils.GetMiddleValue(minSpeed, maxSpeed);
        float angularVelocity = Utils.CalculateAngVelByLinVel(middleSpeed, planeRadius);
        target.GetComponent<TargetConroller>().SetSpeed(angularVelocity);
    }
	
	void Update () {

        // Курс всегда держим на фьючер-таргет
        Vector3 direction = target.FutureTarget.transform.position - transform.position;
        float distance = direction.magnitude;
        float planeCalculatedVelocity = maxSpeed - middleSpeed / (1f + distance);
        float planeClampedVelocity = Mathf.Clamp(planeCalculatedVelocity, minSpeed, maxSpeed);

        // Если самолёт направлен в сторону от таргетов то он снижает скорость 
		Vector3 targetDirection = target.transform.position - transform.position;

        if (Vector3.Angle(transform.up,direction) < 30f && Vector3.Angle(transform.up, targetDirection) < 30f)
        {
            planeClampedVelocity = Mathf.Lerp(planeClampedVelocity,maxSpeed,0.1f);
        }
        else
        {
            planeClampedVelocity = Mathf.Lerp(planeClampedVelocity, minSpeed, 0.1f);
        }

        // При контакте с целью самолёт замедляется
        if (contactedWithTarget)
        {
            planeClampedVelocity = Mathf.Lerp(planeClampedVelocity, minSpeed, 0.1f);
        }

        motor.SetVelocity(planeClampedVelocity);

		Quaternion toRotation = Quaternion.FromToRotation(Vector3.up, direction);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, toRotation, maxRotationSpeed);
        motor.SetRotation(newRotation);

        if (!isLanding)
        {
            // После объявления захода на посадку самолёт определённое время летит на бесконечность
            // для избежания ситуации невозможности захода на посадку из-за малой скорости вращения вокруг оси
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

    void OnTriggerEnter(Collider triggerCollider)
    {
        if (triggerCollider.gameObject == target.gameObject)
        {
            contactedWithTarget = true;
        }

        // Обработка посадки
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
