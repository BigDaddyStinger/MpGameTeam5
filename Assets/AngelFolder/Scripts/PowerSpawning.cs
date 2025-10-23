using System.Collections;
using UnityEngine;

public class PowerSpawning : MonoBehaviour
{
    [Header("Fly Modifiers")]
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
        flyingSpeed = Random.Range(1.5f, 3.5f);
        flyingTime = Random.Range(1.5f, 3f);
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
