using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class KartInput : MonoBehaviour
{
    public float throttle;   // -1/1 (W/S or stick Y)
    public float steer;      // -1/1 (A/D or stick X)

    public bool brakeHeld;
    public bool driftHeld;
    public bool hopPressed;
    public bool _hopQueued;

    public string moveActionName = "Move";
    public string brakeActionName = "Brake";
    public string driftActionName = "Drift";
    public string hopActionName = "Hop";

    PlayerInput _playerInput;
    InputAction _move;
    InputAction _brake;
    InputAction _drift;
    InputAction _hop;

    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        var map = _playerInput?.actions;
        if (map == null) { Debug.LogError("PlayerInput has no Actions asset assigned."); return; }

        _move = map[moveActionName];
        _brake = map[brakeActionName];
        _drift = map[driftActionName];
        _hop = map[hopActionName];

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
        if (_drift != null)
        {
            _drift.performed += _ => driftHeld = true;
            _drift.canceled += _ => driftHeld = false;
        }
        if (_hop != null)
        {
            _hop.performed += _ => _hopQueued = true;
        }
    }

    void OnDestroy()
    {
        if (_move != null) { _move.performed -= OnMove; }
        if (_brake != null) { _brake.performed -= _ => { }; }
        if (_drift != null) { _drift.performed -= _ => { }; }
        if (_hop != null) { _hop.performed -= _ => { }; }
    }

    void Update()
    {
        hopPressed = _hopQueued;
        _hopQueued = false;
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 v = ctx.ReadValue<Vector2>(); // x = A/D, y = W/S
        steer = Mathf.Clamp(v.x, -1f, 1f);
        throttle = Mathf.Clamp(v.y, -1f, 1f);
    }
}
