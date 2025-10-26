using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementV3 : MonoBehaviour
{
    [SerializeField] public int playerScore = 0;
    [SerializeField] int coinAmount = 10;


    [SerializeField] float moveSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float jumpForce;
    [SerializeField] float groundCheckDistance;
    [SerializeField] float turnSpeed;
    [SerializeField] float turnRate;
    [SerializeField] float yawDeg;

    [SerializeField] bool jumpQueued;
    [SerializeField] bool isGrounded;

    [SerializeField] private Vector2 moveInput;

    [SerializeField] Rigidbody rb;

    [SerializeField] LayerMask groundMask;

    [SerializeField] Vector3 currentPlanar;

    private void Awake()
    {
        if (!rb) rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        //========== Movement ==========//

        Vector3 desiredPlanar = (transform.right * moveInput.x + transform.forward * moveInput.y) * moveSpeed;

        currentPlanar = Vector3.MoveTowards(currentPlanar, desiredPlanar, acceleration * Time.fixedDeltaTime);

        Vector3 step = new Vector3(currentPlanar.x, 0f, currentPlanar.z) * Time.fixedDeltaTime;
        
        rb.MovePosition(rb.position + step);


        //========== Turning ==========//

        yawDeg = turnSpeed * moveSpeed * moveInput.x * Time.fixedDeltaTime;

        rb.MoveRotation(Quaternion.Euler(0f, yawDeg, 0f) * rb.rotation);

        Vector3 inputDir = (transform.right * moveInput.x + transform.forward * moveInput.y);

        if (inputDir.sqrMagnitude > 0.0001f)
        {
            Vector3 desired = inputDir.normalized * moveSpeed;

            if (currentPlanar.sqrMagnitude < 0.0001f)
            {
                currentPlanar = desired;
            }
            else
            {
                currentPlanar = Vector3.RotateTowards(
                    currentPlanar,
                    desired,
                    Mathf.Deg2Rad * turnRate * Time.fixedDeltaTime, 
                    0f
                    );
            }
        }


        //========== Jump ==========//

        if (jumpQueued && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpQueued = false;


        //========== Ground Check ==========//

        CheckGround();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) jumpQueued = true;
    }

    public void CheckGround()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    public void AddPoints()
    {
        playerScore += coinAmount;
        if (GameManager.Instance) GameManager.Instance.NotifyScoreChanged();
    }





    public void OnTriggerEnter(Collider other)
    {
        if (other == null) return;

        else if (other.gameObject.CompareTag("Coin"))
        {
            Debug.Log("Hitting Coin");
            Destroy(other.gameObject);
            AddPoints();
        }

        else if (other.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("Hitting Hazard");
            if (GameManager.Instance) GameManager.Instance.NotifyPlayerDied(this);
        }

        else if (other.gameObject.CompareTag("Retard"))
        {
            Debug.Log("Hitting Retard");
        }

        else if (other.gameObject.CompareTag("Boost"))
        {
            Debug.Log("Hitting Boost");
        }
    }
}
