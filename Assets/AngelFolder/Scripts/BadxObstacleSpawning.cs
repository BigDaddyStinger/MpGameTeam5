using System.Collections;
using UnityEngine;

public class BadxObstacleSpawning : MonoBehaviour
{
    [Header("Fly Modifiers")]
    [SerializeField] float flyingSpeed = 3.5f;
    [SerializeField] float flyingTime = 3.5f;
    [Header("Size Modifiers")]
    [SerializeField] float SizeMin = 1f;
    [SerializeField] float SizeMax = 10f;
    Vector3 movement;
    private void Awake()
    {

    }
    private void OnDisable()
    {
        
    }
    private void OnEnable()
    {
        flyingSpeed = Random.Range(0.5f, 2.5f);
        flyingTime = Random.Range(3.5f, 5f);
        transform.localScale = new Vector3(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f));
        StartCoroutine(ObstacleMoveUp());
    }
    IEnumerator ObstacleMoveUp()
    {
        float elapsedTimeInMovement = 0;
        while (flyingTime > elapsedTimeInMovement)
        {
            Debug.Log("Is flaying");
            elapsedTimeInMovement += Time.deltaTime;
            transform.position += new Vector3(0, flyingSpeed, 0) * Time.deltaTime;
            yield return null;
        }
    }


}
