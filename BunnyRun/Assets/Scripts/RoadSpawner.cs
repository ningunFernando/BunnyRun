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
    private Queue<int> recentIndexes = new Queue<int>();

    private int historySize = 4;

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
        int newIndex;

        if (fixedIndex >= 0 && fixedIndex < roadPrefabs.Length)
        {
            newIndex = fixedIndex;
        }
        else
        {
            newIndex = GetUniqueRandomIndex();
        }

        if (recentIndexes.Contains(newIndex))
        {
            Debug.LogWarning($"Repetición detectada: El segmento {newIndex} ya apareció en los últimos {historySize} caminos.");
        }

        // Añadir el nuevo índice al historial
        recentIndexes.Enqueue(newIndex);
        if (recentIndexes.Count > historySize)
        {
            recentIndexes.Dequeue();
        }

        Debug.Log($"Se generó el segmento {newIndex}. Historial actual: [{string.Join(", ", recentIndexes)}]");

        road = Instantiate(roadPrefabs[newIndex]);

        Transform newStart = road.transform.Find("StartPoint");
        Transform newEnd = road.transform.Find("EndPoint");

        if (newStart != null && newEnd != null)
        {
            road.transform.rotation = Quaternion.Euler(0, 180, 0);

            Vector3 offset = newStart.position - road.transform.position;
            road.transform.position = lastEndPoint.position - offset;

            lastEndPoint = newEnd;
        }

        spawnedRoads.Add(road);
    }

    private int GetUniqueRandomIndex()
    {
        int newIndex;
        int attempts = 0;

        do
        {
            newIndex = Random.Range(0, roadPrefabs.Length);
            attempts++;

            Debug.Log($"Intento {attempts}: Probando el segmento {newIndex}");

        } while (recentIndexes.Contains(newIndex) && attempts < 10); 

        return newIndex;
    }

    private void DestroyRoad()
    {
        Destroy(spawnedRoads[0]);
        spawnedRoads.RemoveAt(0);
    }
}
