using UnityEngine;
using UnityEngine.InputSystem;

public class LocalMultiplayerManager : MonoBehaviour
{
    private PlayerInputManager pim;

    private void Awake()
    {
        // Try to cache an instance if it already exists
        pim = PlayerInputManager.instance ?? FindAnyObjectByType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        // Reacquire in case it was created after our Awake
        if (pim == null) pim = PlayerInputManager.instance ?? FindAnyObjectByType<PlayerInputManager>();

        if (pim != null)
            pim.onPlayerJoined += OnPlayerJoined;
        else
            Debug.LogError("LocalMultiplayerManager: No PlayerInputManager found in the scene.");
    }

    private void Start()
    {
        // Final fallback if PIM initialized after OnEnable
        if (pim == null && PlayerInputManager.instance != null)
        {
            pim = PlayerInputManager.instance;
            pim.onPlayerJoined += OnPlayerJoined;
        }
    }

    private void OnDisable()
    {
        if (pim != null) pim.onPlayerJoined -= OnPlayerJoined;
    }

    private void OnPlayerJoined(PlayerInput pi)
    {
        // Ensure the player's camera is bound (if you added PlayerCameraBinder this can be skipped)
        var cam = pi.camera ?? pi.GetComponentInChildren<Camera>(true);
        if (cam == null) return;

        // 2×2 split
        cam.rect = pi.playerIndex switch
        {
            0 => new Rect(0f, 0.5f, 0.5f, 0.5f),
            1 => new Rect(0.5f, 0.5f, 0.5f, 0.5f),
            2 => new Rect(0f, 0f, 0.5f, 0.5f),
            3 => new Rect(0.5f, 0f, 0.5f, 0.5f),
            _ => new Rect(0, 0, 1, 1)
        };

        // Only one AudioListener active (player 0)
        var listener = cam.GetComponent<AudioListener>();
        if (listener) listener.enabled = (pi.playerIndex == 0);
    }
}
