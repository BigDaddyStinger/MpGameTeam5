using System.Collections.Generic;
using UnityEngine;

public class ObstaclePooling : MonoBehaviour
{

    [SerializeField] ObstacleSpawningScript objectPrefab;
    [SerializeField] int amountOfObjects;

    Queue<ObstacleSpawningScript> remainingObjects = new Queue<ObstacleSpawningScript>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < amountOfObjects; i++)
        {
            var b = Instantiate(objectPrefab);
            b.SetPool(this);
            b.gameObject.SetActive(false);
        }
    }

    public void SpawnObject(/*Vector3 spawnLocation*/)
    {
        if (remainingObjects.Count > 0)
        {
            var current = remainingObjects.Dequeue();
            current.gameObject.SetActive(true);
            //current.transform.position = spawnLocation;
        }
    }

    public void AddToQueue(ObstacleSpawningScript b)
    {
        remainingObjects.Enqueue(b);
    }
}

