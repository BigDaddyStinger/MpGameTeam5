using UnityEngine;

public class BoostPad : MonoBehaviour
{
    public float boost = 12f;
    public float duration = 0.6f;

    void OnTriggerEnter(Collider other)
    {
        var motor = other.GetComponentInParent<KartMotor>();
        if (!motor) return;

        var rb = motor.GetComponent<Rigidbody>();
        rb.linearVelocity += motor.transform.forward * boost;
    }
}
