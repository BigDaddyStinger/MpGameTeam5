using System.Collections;
using UnityEngine;

public class HazaredSpawning : MonoBehaviour
{
    [SerializeField] float flyingSpeed = 3.5f;
    [SerializeField] float flyingTime = 3.5f;
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
        float hazardScale = Random.Range(1f, 5f);
        transform.localScale = new Vector3(hazardScale, hazardScale, hazardScale);
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
