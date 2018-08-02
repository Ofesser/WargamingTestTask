using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Displaying free planes logic

public class PlaneCounter : MonoBehaviour {

    [SerializeField]
    private TextMesh textMesh;

    private Ship ship;
    private int planeAwailable = 5;

	private void Start()
	{
        ship = GetComponent<Ship>();
        ship.onPlaneSpawn += SpawnPlane;
        ship.onPlaneDestroy+= DestroyPlane;
	}

	private void Update()
	{
        textMesh.transform.rotation = Quaternion.Euler(0f,0f,0f);
	}

	private void SpawnPlane()
    {
        planeAwailable--;
        textMesh.text = planeAwailable.ToString();
    }

    private void DestroyPlane()
    {
        planeAwailable++;
        textMesh.text = planeAwailable.ToString();
    }
}
