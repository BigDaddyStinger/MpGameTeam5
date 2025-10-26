using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KartInput : MonoBehaviour
{
    public float throttle;   // -1/1 (W/S or stick Y)
    public float steer;      // -1/1 (A/D or stick X)

    public bool brakeHeld;
    public bool jumpPressed;
    public bool jumpQueued;

    public string moveActionName = "Move";
    public string brakeActionName = "Brake";
    public string jumpActionName = "Jump";

    PlayerInput _playerInput;
    InputAction _move;
    InputAction _brake;
    InputAction _jump;

    public Rigidbody _rb;

    void OnEnable()
    {
        var pi = GetComponent<PlayerInput>();
        Debug.Log($"[PI] hasActions={(pi.actions != null)} currentMap={pi.currentActionMap?.name}");

        var map = pi.currentActionMap;
        if (map != null && !map.enabled) { Debug.Log("[PI] map was disabled; enabling"); map.Enable(); }

        _jump = map?["Jump"];
        Debug.Log($"[PI] jumpFound={_jump != null} bindings={_jump?.bindings.Count}");

        if (_jump != null)
        {
            _jump.performed += _ => { Debug.Log("[PI] Jump performed"); jumpQueued = true; };
            _jump.canceled += _ => Debug.Log("[PI] Jump canceled");
        }
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _playerInput = GetComponent<PlayerInput>();
        var map = _playerInput?.actions;
        if (map == null) { Debug.LogError("PlayerInput has no Actions asset assigned."); return; }

        _move = map[moveActionName];
        _brake = map[brakeActionName];
        _jump = map[jumpActionName];

        if (_move != null)
        {
            _move.performed += OnMove;
            _move.canceled += _ => { throttle = 0f; steer = 0f; };
        }
        if (_brake != null)
        {
            _brake.performed += ctx => brakeHeld = ctx.ReadValueAsButton();
            _brake.canceled += _ => brakeHeld = false;
        }
        if (_jump != null)
        {
            _jump.performed += _ => jumpQueued = true;
        }
    }

    void OnDestroy()
    {
        if (_move != null) { _move.performed -= OnMove; }
        if (_brake != null) { _brake.performed -= _ => { }; }
        if (_jump != null) { _jump.performed -= _ => { }; }
    }

    void Update()
    {
        jumpPressed = jumpQueued;
        jumpQueued = false;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>(); // x = A/D, y = W/S
        steer = Mathf.Clamp(v.x, -1f, 1f);
        throttle = Mathf.Clamp(v.y, -1f, 1f);
    }

}
