using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Логика отображения свободных самолётов

public class PlaneCounter : MonoBehaviour {

    [SerializeField]
    private TextMesh textMesh;

    private PlaneSpawner planeSpawner;
    private int planeAwailable = 5;

	private void Start()
	{
        planeSpawner = GetComponent<PlaneSpawner>();
        planeSpawner.onPlaneSpawn += SpawnPlane;
        planeSpawner.onPlaneDestroy+= DestroyPlane;
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
