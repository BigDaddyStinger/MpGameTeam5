using UnityEngine;

[CreateAssetMenu(fileName = "KartStats", menuName = "Scriptable Objects/KartStats")]
public class KartStats : ScriptableObject
{
    [Header("Speed")]
    public float maxSpeed = 22f;
    public float reverseMax = 6f;
    public float accel = 12f;
    public float brakeDecel = 30f;
    public float coastDecel = 4f;

    [Header("Handling")]
    public float steerAtZero = 120f;
    public float steerAtMax = 240f;
    public float grip = 18f;

    [Header("Camera Anchor (local)")]
    public Vector3 camAnchorLocal = new Vector3(0f, 0.8f, -0.3f);
}
