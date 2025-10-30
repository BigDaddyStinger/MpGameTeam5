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
    [SerializeField] float spawnDistanceFromPlayerMin = 2;
    [SerializeField] float spawnDistanceFromPlayerMax = 30;

    [Header("Spawn Object Below Player (Y)")]
    [SerializeField] float spawnBelowFromPlayer = -1;

    [Header("Spawn Object In Front Of Player (X)")]
    [SerializeField] float spawnRangeFromPlayerMin = -20;
    [SerializeField] float spawnRangeFromPlayerMax = 20;

    //References 
    [SerializeField] ObstacleManagerCollective obstacleManagerCollective;
    ObstaclePooling pool;

    [Header("Matt stuff to make director type system work")]
    [SerializeField] private Transform targetPlayer;

    Vector3 locationToSpawnObstacle;


    //Matt added to allow game manager to access spawners
    public void SetTarget(Transform t)
    {
        targetPlayer = t;
    }


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
            Transform t = targetPlayer;
            if (t == null)
            {
                foreach (GameObject p in obstacleManagerCollective.players)
                {
                    if (p != null) { t = p.transform; break; }
                }
                if (t == null) return;
            }

            float forwardDist = Random.Range(spawnDistanceFromPlayerMin, spawnDistanceFromPlayerMax);

            float lateralOffset = Random.Range(spawnRangeFromPlayerMin, spawnRangeFromPlayerMax);

            float verticalOffset = spawnBelowFromPlayer;

            Vector3 spawnPos =
                t.position +
                t.forward * forwardDist +
                t.right * lateralOffset +
                t.up * verticalOffset;

            transform.position = spawnPos;

        }
                
                
                /*    Above Code from Matt. Working on spawning actually in front of the players. 
                    
                    if (p != null)
                {
                    var pLocation = p.transform.position;
                    var pRotation = p.transform.rotation;

                    var pLocationOffSet = new Vector3(pLocation.x + Random.Range(spawnRangeFromPlayerMin, spawnRangeFromPlayerMax), pLocation.y + spawnBelowFromPlayer, pLocation.z + Random.Range(spawnDistanceFromPlayerMin, spawnDistanceFromPlayerMax));
                    locationToSpawnObstacle = pLocationOffSet; 
                    transform.position = locationToSpawnObstacle;
                }

                */
            
        
    }

}
