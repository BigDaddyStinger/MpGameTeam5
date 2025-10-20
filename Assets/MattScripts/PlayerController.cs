using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameSO _gameSO;

    public PlayerControl _playerControls;

    public Rigidbody _rb;

    public Transform _groundCheck;

    [SerializeField] private Collider[] col;

    public LayerMask thisIsGround;

    private Vector2 moveInput;

    public float moveSpeed = 5f;

    public bool isGrounded;


    void OnEnable()
    {
        _playerControls.Enable();
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerControls = new PlayerControl();
        _playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        _playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    void Start()
    {
        
    }

    void Update()
    {
        col = Physics.OverlapSphere(_groundCheck.position, 0.2f, thisIsGround);

        if (col.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.fixedDeltaTime;

        GetComponent<Rigidbody>().MovePosition(transform.position + movement);
    }

    void OnDisable()
    {
        _playerControls.Disable();
    }
}
