using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class ObjectSpawningManager : MonoBehaviour
{
    [SerializeField] GameObject ObjectToSpawn;
    [SerializeField] float spawnTime = 1;
    [Range(0,1)] public float DifficultyMult; 

    [SerializeField] float spawnDistanceFromPlayerMin = 10;
    [SerializeField] float spawnDistanceFromPlayerMax = 30;

    [SerializeField] float spawnBelowFromPlayer = -10;

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

                    Instantiate(ObjectToSpawn, pLocationOffSet, pRotation);
                }
            elapsed = 0;
            }
        }
    }
    
    IEnumerator SpawnObject()
    {
        foreach (GameObject p in players)
        {
            if (p != null)
            {
                var pLocation = p.transform.position;
                var pLocationOffSet = new Vector3(pLocation.x + Random.Range(spawnRangeFromPlayerMin, spawnRangeFromPlayerMax), pLocation.y - spawnBelowFromPlayer, pLocation.z + spawnDistanceFromPlayerMin);
                yield return new WaitForSeconds(spawnTime);
                Instantiate(ObjectToSpawn, pLocationOffSet, p.transform.rotation);
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
