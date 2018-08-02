using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Отвечает за создание таргетов для самолётов

public class TargetSpawner : MonoBehaviour {

    [SerializeField]
    private GameObject targetPrefab;
    [SerializeField]
    private Transform targetContainer;
    [SerializeField]
    private Transform ship;

	private void Update()
	{
        // Положение таргетов не должны зависеть от поворота корабля
        targetContainer.transform.rotation = Quaternion.Euler(0f,0f,0f);
	}

    public TargetConroller SpawnTarget(float planeRadius, float planeIntervalDistance,
                                       Transform planeTransform,LinkedList<GameObject> planesList)
    {
        Vector3 targetStartPosition;
        if (planesList.Count == 0)
        {
            targetStartPosition = ship.position + Vector3.up * planeRadius;
        }
        else
        {
            // planeIntervalDistance - расстояние между самолётами, находящихся на окружности
            // planeRadius - радиус окружности
            // Вычисляем угол дуги с помощью теоремы косинусов и поворачиваем позицию пред. самолёта на этот угол,
            // таким образом находим положение таргета следующего самолёта

            float rotationDistanceAngle = Utils.CalculateAngleOfArc(planeRadius, planeIntervalDistance);
            GameObject lastPlaneTargetObject = planesList.Last.Value.GetComponent<PlaneController>().Target.gameObject;
            Vector3 fromShipToPlaneVector = lastPlaneTargetObject.transform.position - ship.position;
            fromShipToPlaneVector = Quaternion.Euler(0f, 0f, rotationDistanceAngle) * fromShipToPlaneVector;

            targetStartPosition = ship.position + fromShipToPlaneVector;
        }

        GameObject newTarget = Instantiate(targetPrefab,targetContainer);
        newTarget.transform.position = targetStartPosition;

        // Привязываем самолёт к таргету
        newTarget.GetComponent<TargetConroller>().PlaneTransform = planeTransform;

        return newTarget.GetComponent<TargetConroller>();
    }


}
