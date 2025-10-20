using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public PlayerControl _playerControls; // Or whatever you named your generated class

    private Vector2 moveInput;
    public float moveSpeed = 5f; // Adjust as needed

    void Awake()
    {
        _playerControls = new PlayerControl();
        _playerControls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        _playerControls.Player.Move.canceled += ctx => moveInput = Vector2.zero; // Stop movement when input is released
    }

    void FixedUpdate() // For physics-based movement
    {
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.fixedDeltaTime;

        GetComponent<Rigidbody>().MovePosition(transform.position + movement);

    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnEnable()
    {
        _playerControls.Enable();
    }

    void OnDisable()
    {
        _playerControls.Disable();
    }
}
