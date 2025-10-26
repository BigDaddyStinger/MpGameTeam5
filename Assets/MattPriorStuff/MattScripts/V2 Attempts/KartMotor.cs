using UnityEngine;
using UnityEngine.InputSystem;

public class KartMotor : MonoBehaviour
{
    public KartStats stats;
    public Animator _anim;

    float _maxSpeed; 
    float _reverseMax;
    float _accel;
    float _brakeDecel;
    float _coastDecel;
    float _turnRateMin;
    float _turnRateMax;
    float _grip;
    float _jumpForce;

    Rigidbody rb;
    KartInput input;
    Transform camAnchor;

    [SerializeField] float speed;
    Vector3 planarVel;

    [SerializeField] LayerMask groundMask = ~0;
    [SerializeField] Vector3 groundCheckOffset = new Vector3(0f, 0.1f, 0f);
    [SerializeField] float groundCheckRadius = 0.25f;
   // [SerializeField] bool grounded = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<KartInput>();
        _anim = GetComponentInChildren<Animator>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        rb.constraints |= RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        camAnchor = new GameObject("CamTrackPoint").transform;
        camAnchor.SetParent(transform, false);
        camAnchor.localPosition = stats ? stats.camAnchorLocal : new Vector3(0f, 0.8f, -0.3f);

        CacheFromStats();
    }

    public Transform CameraAnchor => camAnchor;

    void CacheFromStats()
    {
        _maxSpeed = stats ? stats.maxSpeed : 22f;
        _reverseMax = stats ? stats.reverseMax : 6f;
        _accel = stats ? stats.accel : 12f;
        _brakeDecel = stats ? stats.brakeDecel : 30f;
        _coastDecel = stats ? stats.coastDecel : 4f;
        _turnRateMin = stats ? stats.steerAtZero : 120f;
        _turnRateMax = stats ? stats.steerAtMax : 240f;
        _grip = stats ? stats.grip : 18f;
        _jumpForce = stats ? stats.jumpForce : 10f;
    }

    void FixedUpdate()
    {
        float throttle = input.throttle; // W/S  (-1/1)
        float steer = input.steer;    // A/D  (-1/1)

        float targetSpeed = (throttle >= 0f) ? throttle * _maxSpeed
                                             : throttle * _reverseMax;

        float rate =
            Mathf.Approximately(throttle, 0f) ? _coastDecel :
            (Mathf.Sign(throttle) < 0f && Mathf.Abs(speed) > 0.1f) ? _brakeDecel :
            _accel;

        speed = Mathf.MoveTowards(speed, targetSpeed, rate * Time.fixedDeltaTime);
        _anim.SetFloat("Speed", speed * .1f);

        //========== Turning Stuff ==========//
        float speed01 = Mathf.Clamp01(Mathf.Abs(speed) / Mathf.Max(0.001f, _maxSpeed));
        float steerRate = Mathf.Lerp(_turnRateMin, _turnRateMax, speed01); // deg/s
        float yawDeg = steer * steerRate * Time.fixedDeltaTime;

        rb.MoveRotation(Quaternion.Euler(0f, yawDeg, 0f) * rb.rotation);


        //========== Velocity ==========//
        Vector3 desiredPlanar = transform.forward * speed;

        //========== Blends Movement ==========//
        Vector3 v = rb.linearVelocity;
        Vector3 vPlanar = new Vector3(v.x, 0f, v.z);
        Vector3 blendedPlan = Vector3.RotateTowards(vPlanar, desiredPlanar,
                              _grip * Mathf.Deg2Rad * Time.fixedDeltaTime, 999f);

        //========== Applies Movement ==========//
        rb.linearVelocity = new Vector3(blendedPlan.x, v.y, blendedPlan.z);
        planarVel = blendedPlan;


        //========= Jump Movement =========
        if (input.jumpPressed && IsGrounded())
        {
            Debug.Log("Jumping");
            Vector3 curVel;
            curVel = rb.linearVelocity;
            if (curVel.y < 0f) curVel.y = 0f;
            rb.linearVelocity = curVel;

            rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Vector3 origin = transform.position + groundCheckOffset;
        return Physics.CheckSphere(origin + Vector3.down * 0.2f, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
    }
}
