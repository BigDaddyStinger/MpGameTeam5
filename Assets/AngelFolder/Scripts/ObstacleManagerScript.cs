using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class ObstacleManagerScript : MonoBehaviour
{
    [Header("What Prefab To Spawn")]
    [SerializeField] GameObject ObjectToSpawn;
    [Header("The Difficulty/Spawn Frequency")]
    [Range(0, 1)] public float DifficultyMult;
    [SerializeField] float spawnTime = 1;



    [Header("References")]
    [SerializeField] ObstaclePooling pool;
    [SerializeField] ObstacleManagerCollective ObstacleManager;

    float elapsed = 0;

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > spawnTime * DifficultyMult)
        {
            foreach (GameObject p in ObstacleManager.players)
            {
                if (p != null)
                    pool.SpawnObject();
            }
            elapsed = 0;
        }
    }
}
