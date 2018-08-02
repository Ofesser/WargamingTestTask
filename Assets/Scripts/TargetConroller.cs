using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Описывает поведение таргетов

public class TargetConroller : MonoBehaviour {

    // Префаб будущего положения таргета (фьючер-таргет)
    [SerializeField]
    private GameObject futureTargetPrefab;

    private float targetSpeed;

    // Коэффициент удаления фьючер-таргета от основного
    public float futureTargetPeriod = 30f;

	private GameObject futureTarget;
    private Transform planeTransform;

    // Флаг, сигнализирующий о начале посадки
	private bool isLanding;
    // Время разворота перед посадкой
    private float landingTime;
    private float landingTimer;
    // Позиция перед началом захода на посадку
	private Vector3 lastTargetPosition;

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
            // Расстояние до фьючер-таргета зависит от расстояния самолёта до основного таргета
            futureTargetPeriod = Vector3.Magnitude(transform.position - planeTransform.transform.position) * 10f;

            futureTarget.transform.localPosition = Quaternion.Euler(0f, 0f, -targetSpeed * futureTargetPeriod) * transform.localPosition;
        }
        else
        {
            // После объявления захода на посадку самолёт определённое время летит на бесконечность
            // для избежания ситуации невозможности захода на посадку из-за малой скорости вращения вокруг оси
            landingTimer += Time.fixedDeltaTime;
            if (landingTimer < landingTime)
            {
                transform.localPosition = lastTargetPosition * 1000f;
            }
            else
            {
                // Таргет выставлен на корабль
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
