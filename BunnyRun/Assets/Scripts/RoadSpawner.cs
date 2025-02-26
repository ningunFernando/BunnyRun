using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [Tooltip("Put the road prefabs here you can drop all of them all at once")]
    public GameObject[] roadPrefabs; 
    [Tooltip("Player transform reference")]
    public Transform player;
    
    public Transform startPoint;
    [Tooltip("Amount of roads that will be spawned all the time")]
    public int initialRoadCount = 5;
    [Tooltip("the amount of elements of the prefabs list that will always spawn in this order")]
    private int fixedSegmentCount = 3;
    public float roadSpeed = 5f;
    [Tooltip("Units that the road will wait to be behind player so it can be destroyed")]
    public int destroyOffset = 3;

    private List<GameObject> spawnedRoads = new List<GameObject>();
    private Transform lastEndPoint;

    void Start()
    {
        lastEndPoint = startPoint;

        for (int i = 0; i < initialRoadCount; i++)
        {
            SpawnRoad(i < fixedSegmentCount ? i : -1);
        }
    }

    void Update()
    {
        MoveRoads();
        CheckAndRecycleRoads();
    }

    private void MoveRoads()
    {
        foreach (var road in spawnedRoads)
        {
            road.transform.position -= new Vector3(0, 0, roadSpeed * Time.deltaTime); 
        }
    }

    private void CheckAndRecycleRoads()
    {
        if (spawnedRoads.Count > 0)
        {
            GameObject firstRoad = spawnedRoads[0];
            Transform firstEnd = firstRoad.transform.Find("EndPoint");

            if (firstEnd != null && firstEnd.position.z < player.position.z - destroyOffset) 
            {
                DestroyRoad();
                SpawnRoad(); 
            }
        }
    }

    public void SpawnRoad(int fixedIndex = -1)
    {
        GameObject road;

        if (fixedIndex >= 0 && fixedIndex < roadPrefabs.Length)
        {
            road = Instantiate(roadPrefabs[fixedIndex]);
        }
        else
        {
            road = Instantiate(roadPrefabs[Random.Range(0, roadPrefabs.Length)]);
        }

        // Obtener StartPoint y EndPoint del nuevo segmento
        Transform newStart = road.transform.Find("StartPoint");
        Transform newEnd = road.transform.Find("EndPoint");

        if (newStart != null && newEnd != null)
        {
            // Rotar 180° en Y
            road.transform.rotation = Quaternion.Euler(0, 180, 0);

            // Ajustar la posición para que el StartPoint del nuevo segmento coincida con el EndPoint del anterior
            Vector3 offset = newStart.position - road.transform.position;
            road.transform.position = lastEndPoint.position - offset;

            // Actualizar la referencia al EndPoint del nuevo segmento
            lastEndPoint = newEnd;
        }

        spawnedRoads.Add(road);
    }

    private void DestroyRoad()
    {
        Destroy(spawnedRoads[0]);
        spawnedRoads.RemoveAt(0);
    }
}
