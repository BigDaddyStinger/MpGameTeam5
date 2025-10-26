using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameSO _gameSO;

    public Rigidbody _rb;

    public Transform _groundCheck;

    [SerializeField] private Collider[] col;
    
    private InputAction moveAction;
    
    private PlayerInput _playerInput;

    public LayerMask thisIsGround;

    private Vector2 _moveInput;

    private Vector3 _planarVelocity;


//    [SerializeField] float moveSpeed = 5f;
    private float _maxSpeed;
    private float _accel;
    private float _brakeDecel;
    private float _coastDecel;
    private float _turnRate;


    private bool _isGrounded;

    void Reset()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
    }

    void Awake()
    {
        if (_rb == null) _rb = GetComponent<Rigidbody>();
        if (_playerInput == null) _playerInput = GetComponent<PlayerInput>();

        // Hook the "Move" action if present
        var moveAction = _playerInput != null ? _playerInput.actions["Move"] : null;
        if (moveAction != null)
        {
            moveAction.performed += OnMove;
            moveAction.canceled += OnMove;
        }
        else
        {
            Debug.LogWarning($"{name}: No 'Move' action found on PlayerInput. Add one (Vector2).");
        }
    }

    private void OnEnable()
    {
        _maxSpeed = _gameSO.maxSpeed;
        _accel = _gameSO.accel;
        _brakeDecel = _gameSO.brakeDecel;
        _coastDecel = _gameSO.coastDecel;
        _turnRate = _gameSO.turnRate;
    }


    void OnDestroy()
    {
        if (_playerInput != null)
        {
            var moveAction = _playerInput.actions["Move"];
            if (moveAction != null)
            {
                moveAction.performed -= OnMove;
                moveAction.canceled -= OnMove;
            }
        }
    }

    void Update()
    {
        if (_groundCheck)
            _isGrounded = Physics.CheckSphere(_groundCheck.position, 0.2f, thisIsGround, QueryTriggerInteraction.Ignore);
        else
            _isGrounded = true; // no ground check assigned → always “grounded”
    }

    // ── Movement (car-like accel/coast/brake) ─────────────────────────────────
    void FixedUpdate()
    {
        // Build desired direction on XZ from input (world-relative; use camera if you prefer)
        Vector3 desiredDir = new Vector3(_moveInput.x, 0f, _moveInput.y);
        float inputMag = desiredDir.magnitude;
        if (inputMag > 1f) inputMag = 1f;
        if (inputMag > 0f) desiredDir /= inputMag;

        // Current planar speed and target speed
        float currentSpeed = new Vector3(_planarVelocity.x, 0f, _planarVelocity.z).magnitude;
        float targetSpeed = inputMag * _maxSpeed;

        // Choose accel/decel rate
        float rate =
            (inputMag == 0f) ? _coastDecel :
            (currentSpeed > 0.01f && Vector3.Dot(_planarVelocity, desiredDir) < 0f) ? _brakeDecel :
            _accel;

        // Ease speed toward target
        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);

        // Turn velocity toward input heading
        if (inputMag > 0f && newSpeed > 0.001f)
        {
            Vector3 from = (currentSpeed > 0.001f) ? _planarVelocity.normalized : desiredDir;
            Vector3 to = Vector3.RotateTowards(from, desiredDir, Mathf.Deg2Rad * _turnRate * Time.fixedDeltaTime, 1f);
            _planarVelocity = to * newSpeed;
        }
        else
        {
            _planarVelocity = (newSpeed > 0.001f) ? _planarVelocity.normalized * newSpeed : Vector3.zero;
        }

        // Apply movement (physics-friendly)
        if (_isGrounded)
            _rb.MovePosition(_rb.position + _planarVelocity * Time.fixedDeltaTime);

        // Optional: face move direction
        if (_planarVelocity.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(_planarVelocity, Vector3.up);
            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, targetRot, _turnRate * Time.fixedDeltaTime));
        }
    }

    // ── Input callback ────────────────────────────────────────────────────────
    private void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    //private void Reset()
    //{
    //    _rb = GetComponent<Rigidbody>();
    //    _playerInput = GetComponent<PlayerInput>();
    //}


    //void Awake()
    //{
    //    if (_rb == null)
    //    {
    //        _rb = GetComponent<Rigidbody>();
    //    }

    //    if (_playerInput == null)
    //    {
    //        _playerInput = GetComponent < PlayerInput)> ();
    //    }

    //    moveAction = _playerInput.actions["Move"];

    //    _maxSpeed = _gameSO.maxSpeed;
    //    _accel = _gameSO.accel;
    //    _brakeDecel = _gameSO.brakeDecel;
    //    _coastDecel = _gameSO.coastDecel;
    //    _turnRate = _gameSO.turnRate;
    //}

    //void OnEnable()
    //{
    //    moveAction.performed += OnMove;
    //    moveAction.canceled += OnMove;
    //}

    //void OnDisable()
    //{
    //    moveAction.performed -= OnMove;
    //    moveAction.canceled -= OnMove;
    //}

    //void Start()
    //{

    //}

    //void Update()
    //{
    //    col = Physics.OverlapSphere(_groundCheck.position, 0.2f, thisIsGround);

    //    if (col.Length > 0)
    //    {
    //        isGrounded = true;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //    }
    //}

    //void FixedUpdate()
    //{
    //    {
    //        // ... compute desiredDir & inputMag as before

    //        float targetSpeed = inputMag * _maxSpeed;
    //        float currentSpeed = new Vector3(planarVelocity.x, 0f, planarVelocity.z).magnitude;

    //        float rate =
    //            (inputMag == 0f) ? _coastDecel :
    //            (currentSpeed > 0.01f && Vector3.Dot(planarVelocity, desiredDir) < 0f) ? _brakeDecel :
    //            _accel;

    //        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, rate * Time.fixedDeltaTime);

    //        // Turn toward input using _turnRate, then MovePosition, same as before…
    //        Vector3 from = (currentSpeed > 0.001f) ? planarVelocity.normalized : desiredDir;
    //        Vector3 to = (inputMag > 0f) ? Vector3.RotateTowards(from, desiredDir, Mathf.Deg2Rad * _turnRate * Time.fixedDeltaTime, 1f) : from;
    //        planarVelocity = (newSpeed > 0.001f) ? to * newSpeed : Vector3.zero;

    //        _rb.MovePosition(_rb.position + planarVelocity * Time.fixedDeltaTime);
    //        if (planarVelocity.sqrMagnitude > 0.0001f)
    //            _rb.MoveRotation(Quaternion.RotateTowards(_rb.rotation, Quaternion.LookRotation(planarVelocity), _turnRate * Time.fixedDeltaTime));
    //    }
    //}

    //private void OnMove(InputAction.CallbackContext ctx)
    //{
    //    moveInput = ctx.ReadValue<Vector2>();
    //}


}
