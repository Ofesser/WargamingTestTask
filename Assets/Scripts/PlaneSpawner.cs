using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Отвечает за создание самолётов и инициирует начало создания таргетов для них

public class PlaneSpawner : MonoBehaviour {

    [SerializeField]
    private float minSpeed = 1f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private float maxRotationSpeed = 5f; // Максимальная корость вращения вокруг оси самолёта
    [SerializeField]
    private float planeRadius = 3f; // Радиус вращения вокруг корабля
    [SerializeField]
    private float planesDistance = 3f; // Дистанция между самолётами
    [SerializeField]
    private float maxFlightTime = 20f; // Время полёта после которого самолёт начинает заход на посадку

    [SerializeField]
    private GameObject planePrefab;
    [SerializeField]
    private Transform ship;

    private LinkedList<GameObject> planesList;
    private TargetSpawner targetSpawner;

    public Action onPlaneSpawn = delegate {};
    public Action onPlaneDestroy = delegate {};

	private void Start()
	{
        targetSpawner = GetComponent<TargetSpawner>();
        planesList = new LinkedList<GameObject>();
	}

	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            SpawnPlane();
        }
    }

    public void SpawnPlane()
    {
        if (planesList.Count < 5)
        {
            // Стартуем с корабля
            Vector3 newPlanePosition = ship.transform.position;
            GameObject newPlane = Instantiate(planePrefab,newPlanePosition,Quaternion.identity);
            // Передаём параметры самолёту
            newPlane.GetComponent<PlaneController>().SetPlaneParameters(minSpeed, maxSpeed, maxRotationSpeed,
                                                                        planeRadius,maxFlightTime);

            // Создаём цель для самолёта и передаём ей нужные параметры
            TargetConroller spawnedTarget = 
                targetSpawner.SpawnTarget(planeRadius, planesDistance, newPlane.transform,planesList);

            // Привязываем цель к самолёту
            newPlane.GetComponent<PlaneController>().Target = spawnedTarget;
            // Вешаем обработчик окончания полёта
            newPlane.GetComponent<PlaneController>().LandingDone += RemovePlaneFromList;

            planesList.AddLast(newPlane);
            onPlaneSpawn();
        }
    }

    public void RemovePlaneFromList(GameObject plane)
    {
        planesList.Remove(plane);
        onPlaneDestroy();
    }

}
