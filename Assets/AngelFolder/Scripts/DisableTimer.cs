using System.Collections;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    [Header("Disable Time Standard")]
    [SerializeField] float disableTime;

    [Header("Disable Time Between two values")]
    [SerializeField] bool doRandomTime;
    [SerializeField] float disableTimeRandomMin;
    [SerializeField] float disableTimeRandomMax;

    void Start()
    {
        StartCoroutine(DisableObject());
    }
    IEnumerator DisableObject()
    {
        if (doRandomTime)
            yield return new WaitForSeconds(Random.Range(disableTimeRandomMin, disableTimeRandomMax));
        else
            yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }
}