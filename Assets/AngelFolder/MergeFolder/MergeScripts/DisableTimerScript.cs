using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DisableTimerScript : MonoBehaviour
{
    [Header("Disable Time Standard")]
    [SerializeField] float disableTime;

    [Header("Disable Time Between two values")]
    [SerializeField] bool doRandomTime;
    [SerializeField] float disableTimeRandomMin;
    [SerializeField] float disableTimeRandomMax;
    private void OnEnable()
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