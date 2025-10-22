using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class ObjectSpawningManager : MonoBehaviour
{
    [Header("What Prefab To Spawn")]
    [SerializeField] GameObject ObjectToSpawn;
    [Header("The Difficulty/Spawn Frequency")]
    [Range(0,1)] public float DifficultyMult;
    [SerializeField] float spawnTime = 1;

    [Header("Spawn Object Away From Player (Z)")]
    [SerializeField] float spawnDistanceFromPlayerMin = 10;
    [SerializeField] float spawnDistanceFromPlayerMax = 30;

    [Header("Spawn Object Below Player (Y)")]
    [SerializeField] float spawnBelowFromPlayer = -10;

    [Header("Spawn Object In Front Of Player (X)")]
    [SerializeField] float spawnRangeFromPlayerMin = -50 ;
    [SerializeField] float spawnRangeFromPlayerMax = 50;

    [SerializeField] GameObject[] players = new GameObject[4];

    float elapsed = 0;
    private void Awake()
    {
        FindPlayers();
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > spawnTime * DifficultyMult)
        {
            foreach (GameObject p in players)
            {
                if (p != null)
                {
                    var pLocation = p.transform.position;
                    var pRotation = p.transform.rotation;

                    var pLocationOffSet = new Vector3(pLocation.x + Random.Range(spawnRangeFromPlayerMin, spawnRangeFromPlayerMax), pLocation.y + spawnBelowFromPlayer, pLocation.z + Random.Range(spawnDistanceFromPlayerMin, spawnDistanceFromPlayerMax));
                    
                    Instantiate(ObjectToSpawn, pLocationOffSet, Quaternion.identity);
                }
            elapsed = 0;
            }
        }
    }
    void FindPlayers()
    {
        for (int i = 0; i < players.Length; i++)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}
