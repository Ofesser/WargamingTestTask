using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main contoll

public class Ship : MonoBehaviour
{

    [SerializeField]
    private float planeMinSpeed = 1f;
    [SerializeField]
    private float planeMaxSpeed = 10f;
    [SerializeField]
    private float planeMaxRotationSpeed = 5f; // Maximum rotational speed of the plane
    [SerializeField]
    private float planeRadius = 3f; // The radius of rotation around the ship
    [SerializeField]
    private float planesDistance = 3f; // The distance between planes
    [SerializeField]
    private float planeMaxFlightTime = 20f; // Max time before the plane starts approaching
    [SerializeField]
    private float shipAcceleration = 0.01f;
    [SerializeField]
    private float shipRotationSpeed = 1f;

    [SerializeField]
    private GameObject planePrefab;
    [SerializeField]
    private GameObject targetPrefab;
    [SerializeField]
    private Transform targetContainer;

    private LinkedList<GameObject> planesList;

    public Action onPlaneSpawn = delegate { };
    public Action onPlaneDestroy = delegate { };


    private void Start()
    {
        planesList = new LinkedList<GameObject>();
    }

    private void Update()
    {
        // Target positions shoudn't be addicted of the ship rotation
        targetContainer.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }


    #region ShipArea

    public float ShipAcceleration
    { 
        get { return shipAcceleration; }
    }

    public float ShipRotationSpeed
    {
        get { return shipRotationSpeed; }
    }

    #endregion


    #region PlanesArea

    public void SpawnPlane()
    {
        if (planesList.Count < 5)
        {
            // Start from ship
            Vector3 newPlanePosition = transform.position;
            GameObject newPlane = Instantiate(planePrefab, newPlanePosition, Quaternion.identity);

            // Sending parameters to the plane
            newPlane.GetComponent<PlaneController>().
                    SetPlaneParameters(planeMinSpeed, planeMaxSpeed,
                                       planeMaxRotationSpeed, planeRadius, planeMaxFlightTime);

            // Creating target for the plane and send it required parameters
            TargetConroller spawnedTarget = SpawnTarget(newPlane.transform);

            // Linking target to the plane
            newPlane.GetComponent<PlaneController>().Target = spawnedTarget;
            //  End of flight event handler
            newPlane.GetComponent<PlaneController>().LandingDone += RemovePlaneFromList;

            planesList.AddLast(newPlane);
            onPlaneSpawn();
        }
    }

    private void RemovePlaneFromList(GameObject plane)
    {
        planesList.Remove(plane);
        onPlaneDestroy();
    }

    #endregion


    #region TargetsArea

    private TargetConroller SpawnTarget(Transform planeTransform)
    {
        Vector3 targetStartPosition;
        if (planesList.Count == 0)
        {
            targetStartPosition = transform.position + Vector3.up * planeRadius;
        }
        else
        {
            // planeIntervalDistance - distance between planes that flying on circle
            // planeRadius - cirlce radius
            // Calculating the angle of arc with cosine theorem and rotating position of the last plane on this angle
            // and this way we can find the position of target of the next plane

            float rotationDistanceAngle = Utils.CalculateAngleOfArc(planeRadius, planesDistance);
            GameObject lastPlaneTargetObject = planesList.Last.Value.GetComponent<PlaneController>().Target.gameObject;
            Vector3 fromShipToPlaneVector = lastPlaneTargetObject.transform.position - transform.position;
            fromShipToPlaneVector = Quaternion.Euler(0f, 0f, rotationDistanceAngle) * fromShipToPlaneVector;

            targetStartPosition = transform.position + fromShipToPlaneVector;
        }

        GameObject newTarget = Instantiate(targetPrefab, targetContainer);
        newTarget.transform.position = targetStartPosition;

        // Linking the plane to the target
        newTarget.GetComponent<TargetConroller>().PlaneTransform = planeTransform;

        return newTarget.GetComponent<TargetConroller>();
    }

    #endregion

}
