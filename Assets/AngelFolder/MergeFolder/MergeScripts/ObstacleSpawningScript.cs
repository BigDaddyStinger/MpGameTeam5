using System.Collections;
using UnityEngine;

public class ObstacleSpawningScript : MonoBehaviour
{
    [Header("Fly Modifiers")]
    float flyingSpeed;
    [SerializeField] protected float flyingSpeedMin = 0f;
    [SerializeField] protected float flyingSpeedMax = 0f;
    [Header("----------")]
    float flyingTime;
    [SerializeField] protected float flyingTimeMin = 0f;
    [SerializeField] protected float flyingTimedMax = 0f;
    [Header("Size Modifiers")]
    [SerializeField] protected float SizeMin = 1f;
    [SerializeField] protected float SizeMax = 1f;

    [Header("Spawn Object Away From Player (Z)")]
    [SerializeField] float spawnDistanceFromPlayerMin = 10;
    [SerializeField] float spawnDistanceFromPlayerMax = 30;

    [Header("Spawn Object Below Player (Y)")]
    [SerializeField] float spawnBelowFromPlayer = -10;

    [Header("Spawn Object In Front Of Player (X)")]
    [SerializeField] float spawnRangeFromPlayerMin = -50;
    [SerializeField] float spawnRangeFromPlayerMax = 50;

    //References 
    [SerializeField] ObstacleManagerCollective obstacleManagerCollective;
    ObstaclePooling pool;

    Vector3 locationToSpawnObstacle;
    private void Awake()
    {
        obstacleManagerCollective = FindAnyObjectByType<ObstacleManagerCollective>();

    }
    private void OnDisable()
    {
        if (pool != null)
        {
            pool.AddToQueue(this);
        }
    }
    private void OnEnable()
    {
        SetSpawnLocation();
        DoChangesWhenSpawn();
        StartCoroutine(ObstacleMoveUp());
    }
    protected IEnumerator ObstacleMoveUp()
    {
        float elapsedTimeInMovement = 0;
        while (flyingTime > elapsedTimeInMovement)
        {
            //Debug.Log("Is flaying");
            elapsedTimeInMovement += Time.deltaTime;
            transform.position += new Vector3(0, flyingSpeed, 0) * Time.deltaTime;
            yield return null;
        }
    }
    protected void DoChangesWhenSpawn()
    {
        flyingSpeed = Random.Range(flyingSpeedMin, flyingSpeedMax);
        flyingTime = Random.Range(flyingTimeMin, flyingTimedMax);
        transform.localScale = new Vector3(Random.Range(SizeMin, SizeMax), Random.Range(SizeMin, SizeMax), Random.Range(SizeMin, SizeMax));
    }
    public void SetPool(ObstaclePooling bp)
    {
        pool = bp;
    }
    public void SetSpawnLocation()
    {
        foreach (ObstacleManagerScript m in obstacleManagerCollective.managers)
        {
            foreach (GameObject p in obstacleManagerCollective.players)
            {
                if (p != null)
                {
                    var pLocation = p.transform.position;
                    var pRotation = p.transform.rotation;

                    var pLocationOffSet = new Vector3(pLocation.x + Random.Range(spawnRangeFromPlayerMin, spawnRangeFromPlayerMax), pLocation.y + spawnBelowFromPlayer, pLocation.z + Random.Range(spawnDistanceFromPlayerMin, spawnDistanceFromPlayerMax));
                    locationToSpawnObstacle = pLocationOffSet; 
                    transform.position = locationToSpawnObstacle;
                }
            }
        }
    }

}
